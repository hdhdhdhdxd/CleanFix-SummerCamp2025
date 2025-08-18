using System.Reflection;
using Domain.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApi.BaseDatos;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    public new DbSet<T> Set<T>() where T : class, IEntity
    {
        return base.Set<T>();
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Solicitation> Solicitations { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Incidence> Incidences { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
