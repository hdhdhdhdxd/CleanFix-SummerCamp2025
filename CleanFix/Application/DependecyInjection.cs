using System.Reflection;
using Application.Apartments.Queries.GetAparmets;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IGetApartmentsQuery, GetApartmentsQuery>();
    }
}
