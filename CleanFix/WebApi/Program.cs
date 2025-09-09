using Application.Common.Interfaces;
using Infrastructure.Data;
using WebApi.CoreBot;
using WebApi.Infrastructure;
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

builder.Services.AddScoped<IUser, CurrentUser>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Configuración de CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "https://cozyhousesc25.netlify.app/",
            "https://speculab.netlify.app",
            "https://clean-fix-summer-camp2025.vercel.app",
            "http://localhost:4200"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize database and seed data


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();

    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseExceptionHandler(options => { });

app.MapControllers();
app.Run();
