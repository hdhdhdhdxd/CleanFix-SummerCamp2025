using Application.Common.Interfaces;
using Infrastructure.Common.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.BaseDatos;
using WebApi.Services;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DatabaseContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IDatabaseContext>(provider => provider.GetRequiredService<DatabaseContext>());

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<IApartmentRepository, ApartmentService>();
        builder.Services.AddScoped<ICompanyRepository, CompanyService>();
        builder.Services.AddScoped<IMaterialRepository, MaterialService>();
        builder.Services.AddScoped<IRequestRepository, RequestService>();
        builder.Services.AddScoped<IIncidenceRepository, IncidenceService>();
        builder.Services.AddScoped<ICompletedTaskRepository, CompletedTaskService>();  // Si hay error, comentar esta línea
    }
}
