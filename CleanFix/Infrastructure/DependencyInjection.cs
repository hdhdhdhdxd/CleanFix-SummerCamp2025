using System.Text;
using Application.Common.Interfaces;
using Infrastructure.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Identity.Abstracts;
using Infrastructure.Identity.Processors;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
        builder.Services.AddScoped<ISolicitationRepository, SolicitationService>();
        builder.Services.AddScoped<IIncidenceRepository, IncidenceService>();
        builder.Services.AddScoped<ICompletedTaskRepository, CompletedTaskService>();

        // Authentication & Authorization
        builder.Services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();

        builder.Services.Configure<JwtOptions>(
            builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddJwtBearer(options =>
        {
            var jwtOptions = builder.Configuration.GetSection(JwtOptions.JwtOptionsKey)
            .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["ACCESS_TOKEN"];
                    return Task.CompletedTask;
                }
            };
        });

        builder.Services.AddAuthorization();

        //Identity
        builder.Services.AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.Password.RequireDigit = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequiredLength = 8;
            opt.User.RequireUniqueEmail = true;
        }).AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<DatabaseContext>();

        builder.Services.AddTransient<IIdentityService, IdentityService>();
    }
}
