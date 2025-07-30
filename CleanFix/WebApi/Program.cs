using WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApi.BaseDatos;
using WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContextoBasedatos>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IApartment, ApartmentService>();
builder.Services.AddScoped<ISolicitation, SolicitationService>();
builder.Services.AddScoped<ICompany, CompanyService>();
builder.Services.AddScoped<IMaterial, MaterialService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
