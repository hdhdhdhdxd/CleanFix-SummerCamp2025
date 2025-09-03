using Bogus;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.CoreBot;
using WebApi.Services;

// Añadir licencia Community para QuestPDF
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructureServices();
builder.AddApplicationServices();

builder.Services.AddHttpContextAccessor();

// Servicios bot
builder.Services.AddControllers();
// Usar CleanFixBotService como implementación de IBotService
builder.Services.AddScoped<IBotService, CleanFixBotService>();

// Registrar AssistantService para IA LLM
builder.Services.AddScoped<WebApi.Services.IAssistantService, WebApi.Services.AssistantService>();

// Registro de servicios para nuevas funcionalidades
builder.Services.AddScoped<IFacturaPdfService, FacturaPdfService>();
builder.Services.AddScoped<IIncidenciaService, IncidenciaService>();

// Configuración de CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seeding de datos de Apartamento, Edificio y Distrito, y migración automática solo en desarrollo
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();


    var issueTypes = new List<IssueType>
        {
            new IssueType { Name = "Fontanería" },
            new IssueType { Name = "Electricidad" },
            new IssueType { Name = "Carpintería" },
            new IssueType { Name = "Pintura" },
            new IssueType { Name = "Suelos" },
            new IssueType { Name = "Limpieza" }
        };
    /*
    // Seeding de IssueTypes SIEMPRE si la tabla está vacía
    if (!db.IssueTypes.Any())
    {
        db.IssueTypes.AddRange(issueTypes);
        db.SaveChanges();
    }*/


    // Solo hacer seeding si las tablas están vacías
    if (!db.Apartments.Any() && !db.Companies.Any() && !db.Materials.Any() && !db.CompletedTasks.Any())
    {
        db.Database.Migrate();

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
        db.Materials.AddRange(materials);
        db.SaveChanges();

        // Crear 10 usuarios
        var userFaker = new Faker<User>()
            .RuleFor(e => e.Name, f =>
            {
                var type = f.Random.Int(0, 6);
                return $"User {f.UniqueIndex + 1}";
            });
        var users = userFaker.Generate(10);
        db.Users.AddRange(users);
        db.SaveChanges();

        // Crear 10 empresas
        var companyFaker = new Faker<Company>()
            .RuleFor(e => e.IssueType, f => f.PickRandom(issueTypes))
            .RuleFor(e => e.Name, (f, e) => f.Name.LastName()) // Nombre de una sola palabra
            .RuleFor(e => e.Address, f => $"{f.Address}")
            .RuleFor(e => e.Number, f => f.Random.Int(100000000, 999999999).ToString())
            .RuleFor(e => e.Email, f => $"empresa{f.UniqueIndex + 1}@test.com")
            .RuleFor(e => e.Price, f => Math.Round((decimal)f.Random.Float(20, 500), 2))
            .RuleFor(e => e.WorkTime, f => f.Random.Int(1, 30));
        var companies = companyFaker.Generate(200);
        db.Companies.AddRange(companies);
        db.SaveChanges();

        // Crear 10 apartamentos
        var apartmentFaker = new Faker<Apartment>()
            .RuleFor(e => e.FloorNumber, f => f.Random.Int(1, 9))
            .RuleFor(e => e.Address, f => $"{f.Address}")
            .RuleFor(e => e.Surface, f => f.Random.Int(50, 200))
            .RuleFor(e => e.RoomNumber, f => f.Random.Int(3, 7))
            .RuleFor(e => e.BathroomNumber, f => f.Random.Int(1, 3));
        var apartments = apartmentFaker.Generate(10);
        db.Apartments.AddRange(apartments);
        db.SaveChanges();

        

        // Crear 10 solicitudes
        var solicitationStatusOptions = new[] { "In progress", "Ready", "Waiting" };
        var solicitationFaker = new Faker<Solicitation>()
            .RuleFor(e => e.Description, f => f.Lorem.Sentence())
            .RuleFor(e => e.Date, f => f.Date.Recent())
            .RuleFor(e => e.Address, f => f.Address.FullAddress())
            .RuleFor(e => e.MaintenanceCost, f => f.Random.Double(50, 1000))
            .RuleFor(e => e.IssueType, f => f.PickRandom(issueTypes))
            .RuleFor(e => e.ApartmentAmount, f => f.Random.Int(1, 100))
            .RuleFor(e => e.RequestId, f => f.Random.Int(1, 9999));
        var solicitations = solicitationFaker.Generate(30);
        db.Solicitations.AddRange(solicitations);
        db.SaveChanges();

        // Crear 10 incidencias
        var statusOptions = new[] { "In progress", "Ready", "Waiting" };
        var incidenceFaker = new Faker<Incidence>()
            .RuleFor(e => e.IssueType, f => f.PickRandom(issueTypes))
            .RuleFor(e => e.Date, f => f.Date.Recent())
            .RuleFor(e => e.Description, f => f.Lorem.Sentence())
            .RuleFor(e => e.Priority, f => f.PickRandom<Priority>())
            .RuleFor(e => e.Surface, f => f.Random.Int(20, 200))
            .RuleFor(e => e.Address, f => f.Address.FullAddress());
        var incidences = incidenceFaker.Generate(30);
        db.Incidences.AddRange(incidences);
        db.SaveChanges();

        // Crear 10 tareas completadas
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
        db.CompletedTasks.AddRange(completedTasks);
        db.SaveChanges();*/
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
