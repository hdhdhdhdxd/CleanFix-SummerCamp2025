using Bogus;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<DatabaseContextInitialiser>();

        await initialiser.SeedAsync();
    }
}

public class DatabaseContextInitialiser
{
    private readonly DatabaseContext _context;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public DatabaseContextInitialiser(
        DatabaseContext context, 
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        // Migrate database
        await _context.Database.MigrateAsync();

        // Seed default roles and users
        await SeedRolesAsync();
        await SeedUsersAsync();

        var issueTypes = new List<IssueType>
        {
            new IssueType { Name = "Fontanería" },
            new IssueType { Name = "Electricidad" },
            new IssueType { Name = "Carpintería" },
            new IssueType { Name = "Pintura" },
            new IssueType { Name = "Suelos" },
            new IssueType { Name = "Limpieza" },
            new IssueType { Name = "Mantenimiento General"}
        };

        // Seeding de IssueTypes SIEMPRE si la tabla está vacía
        if (!_context.IssueTypes.Any())
        {
            _context.IssueTypes.AddRange(issueTypes);
            await _context.SaveChangesAsync();
        }

        // Solo hacer seeding si las tablas están vacías
        if (!_context.Apartments.Any() && !_context.Companies.Any() && !_context.Materials.Any() && !_context.CompletedTasks.Any())
        {
            // Crear 80 materiales con nombres únicos de una sola palabra (apellido)
            var uniqueMaterialNames = new HashSet<string>();
            var materialNamesFaker = new Faker();
            while (uniqueMaterialNames.Count < 80)
            {
                uniqueMaterialNames.Add(materialNamesFaker.Name.LastName());
            }
            var materialNameList = uniqueMaterialNames.ToList();
            var materialFaker = new Faker<Material>()
                .RuleFor(e => e.IssueType, f => f.PickRandom(issueTypes))
                .RuleFor(e => e.Cost, f => Math.Round((decimal)f.Random.Float(10, 100), 2))
                .RuleFor(e => e.CostPerSquareMeter, f => Math.Round((decimal)f.Random.Float(1, 3), 2))
                .RuleFor(e => e.Available, f => true);
            var materials = new List<Material>();
            for (int i = 0; i < 80; i++)
            {
                var mat = materialFaker.Generate();
                mat.Name = materialNameList[i];
                materials.Add(mat);
            }
            _context.Materials.AddRange(materials);
            await _context.SaveChangesAsync();

            // Crear 10 usuarios
            var userFaker = new Faker<User>()
                .RuleFor(e => e.Name, f =>
                {
                    var type = f.Random.Int(0, 6);
                    return $"User {f.UniqueIndex + 1}";
                });
            var users = userFaker.Generate(10);
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Crear 200 empresas
            var companyFaker = new Faker<Company>()
                .RuleFor(e => e.IssueType, f => f.PickRandom(issueTypes))
                .RuleFor(e => e.Name, (f, e) => f.Name.LastName()) // Nombre de una sola palabra
                .RuleFor(e => e.Address, f => $"{f.Address}")
                .RuleFor(e => e.Number, f => f.Random.Int(100000000, 999999999).ToString())
                .RuleFor(e => e.Email, f => $"empresa{f.UniqueIndex + 1}@test.com")
                .RuleFor(e => e.Price, f => Math.Round((decimal)f.Random.Float(20, 500), 2))
                .RuleFor(e => e.WorkTime, f => f.Random.Int(1, 30));
            var companies = companyFaker.Generate(200);
            _context.Companies.AddRange(companies);
            await _context.SaveChangesAsync();

            // Crear 10 apartamentos
            var apartmentFaker = new Faker<Apartment>()
                .RuleFor(e => e.FloorNumber, f => f.Random.Int(1, 9))
                .RuleFor(e => e.Address, f => $"{f.Address}")
                .RuleFor(e => e.Surface, f => f.Random.Int(50, 200))
                .RuleFor(e => e.RoomNumber, f => f.Random.Int(3, 7))
                .RuleFor(e => e.BathroomNumber, f => f.Random.Int(1, 3));
            var apartments = apartmentFaker.Generate(10);
            _context.Apartments.AddRange(apartments);
            await _context.SaveChangesAsync();

            // Crear 30 solicitudes
            var solicitationStatusOptions = new[] { "In progress", "Ready", "Waiting" };
            var solicitationFaker = new Faker<Solicitation>()
                .RuleFor(e => e.Description, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Recent())
                .RuleFor(e => e.Address, f => f.Address.FullAddress())
                .RuleFor(e => e.IssueType, f => f.PickRandom(issueTypes))
                .RuleFor(e => e.ApartmentAmount, f => f.Random.Int(1, 100))
                .RuleFor(e => e.BuildingCode, f => f.Random.AlphaNumeric(8).ToUpper());
            var solicitations = solicitationFaker.Generate(30);
            _context.Solicitations.AddRange(solicitations);
            await _context.SaveChangesAsync();

            // Crear 30 incidencias
            var statusOptions = new[] { "In progress", "Ready", "Waiting" };
            var incidenceFaker = new Faker<Incidence>()
                .RuleFor(e => e.IssueType, f => f.PickRandom(issueTypes))
                .RuleFor(e => e.Date, f => f.Date.Recent())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence())
                .RuleFor(e => e.Priority, f => f.PickRandom<Priority>())
                .RuleFor(e => e.Surface, f => f.Random.Int(20, 200))
                .RuleFor(e => e.Address, f => f.Address.FullAddress());
            var incidences = incidenceFaker.Generate(30);
            _context.Incidences.AddRange(incidences);
            await _context.SaveChangesAsync();

            // Crear tareas completadas (commented out as in original)
            /* var completedTaskFaker = new Faker<CompletedTask>()
                .RuleFor(e => e.ApartmentId, f => apartments[f.Random.Int(0, apartments.Count - 1)].Id)
                .RuleFor(e => e.Company, f => companies[f.Random.Int(0, companies.Count - 1)])
                .RuleFor(e => e.CreationDate, f => f.Date.Recent())
                .RuleFor(e => e.Price, f => f.Random.Float(50, 1000))
                .RuleFor(e => e.Address, f => f.Address.FullAddress())
                .RuleFor(e => e.IssueTypeId, f => f.Random.Int(1, 6))
                .RuleFor(e => e.Materials, f => materials.OrderBy(x => f.Random.Int()).Take(f.Random.Int(1, 5)).ToList())
                .RuleFor(e => e.Surface, f => f.Random.Int(50, 200))
                .RuleFor(e => e.IsSolicitation, f => f.Random.Bool());
            var completedTasks = completedTaskFaker.Generate(5);
            _context.CompletedTasks.AddRange(completedTasks);
            await _context.SaveChangesAsync();*/
        }
    }

    private async Task SeedRolesAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole<Guid>(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }
    }

    private async Task SeedUsersAsync()
    {
        // Default administrator user
        var administrator = ApplicationUser.Create("admin@cleanfix.com");
        var administratorRole = await _roleManager.FindByNameAsync(Roles.Administrator);

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Password123.");
            if (!string.IsNullOrWhiteSpace(administratorRole?.Name))
            {
                await _userManager.AddToRoleAsync(administrator, administratorRole.Name);
            }
        }
    }
}
