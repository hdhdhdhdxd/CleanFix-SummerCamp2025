using Bogus;
using Microsoft.EntityFrameworkCore;
using WebApi.BaseDatos;
using Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructureServices();
builder.AddApplicationServices();

// Configuración de CORS para permitir localhost:4200
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularLocalhost",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Seeding de datos de Apartamento, Edificio y Distrito, y migración automática solo en desarrollo
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

    if (!db.Apartments.Any() && !db.Companies.Any() && !db.Materials.Any() && !db.Solicitations.Any())
    {
        db.Database.Migrate();

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
        Random random = new Random();
        var type = random.Next(0, 7);
        var companyFaker = new Faker<Company>()
            .RuleFor(e => e.Name, f => {
                var type = f.Random.Int(0, 6);
                return $"Empresa {f.UniqueIndex + 1}_{type}";
            })
            .RuleFor(e => e.Address, f => $"{f.Address}")
            .RuleFor(e => e.Number, f => f.Random.Int(100000000, 999999999).ToString())
            .RuleFor(e => e.Email, f => $"empresa{f.UniqueIndex + 1}@test.com")
            .RuleFor(e => e.Type, f => (IssueType)f.Random.Int(0, 6))
            .RuleFor(e => e.Price, f => f.Random.Int(20, 500))
            .RuleFor(e => e.WorkTime, f => f.Random.Int(20, 500));
        var companies = companyFaker.Generate(10);
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

        // Crear 10 materiales
        Random random2 = new Random();
        var type2 = random2.Next(0, 7);
        var materialFaker = new Faker<Material>()
            .RuleFor(e => e.Name, (f) => {
                var type = f.Random.Int(0, 6);
                return $"Material {f.UniqueIndex + 1} _ {type}";
            })
            .RuleFor(e => e.Cost, f => (decimal)f.Random.Float(10, 1000))
            .RuleFor(e => e.Available, f => true)
            .RuleFor(e => e.Issue, f => (IssueType)f.Random.Int(0, 6));
        var materials = materialFaker.Generate(10);
        db.Materials.AddRange(materials);
        db.SaveChanges(); 

        // Crear 10 solicitudes
        var solicitationFaker = new Faker<Solicitation>()
            .RuleFor(e => e.Apartment, f => apartments[f.Random.Int(0, apartments.Count - 1)])
            .RuleFor(e => e.Company, f => companies[f.Random.Int(0, companies.Count - 1)])
            .RuleFor(e => e.Date, f => f.Date.Recent())
            .RuleFor(e => e.Price, f => f.Random.Float(50, 1000))
            .RuleFor(e => e.Duration, f => f.Random.Float(1, 10))
            .RuleFor(e => e.Address, f => $"{f.Address}")
            .RuleFor(e => e.Type, f => (IssueType)f.Random.Int(0, 6))
            .RuleFor(e => e.Materials, f => materials.OrderBy(x => f.Random.Int()).Take(f.Random.Int(1, 5)).ToList());
        var solicitations = solicitationFaker.Generate(10);
        db.Solicitations.AddRange(solicitations);
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//
app.UseHttpsRedirection();

// Usar la política CORS antes de Authorization
app.UseCors("AllowAngularLocalhost");

app.UseAuthorization();

app.MapControllers();

app.Run();
