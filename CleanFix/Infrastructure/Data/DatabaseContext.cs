using System.Collections.Generic;
using Dominio.Common.Interfaces;
using Dominio.Maintenance;
using Infrastrucure.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastrucure.Data;
public class DatabaseContext : DbContext, IDatabaseContext
{
    public DbSet<Apartment> Apartments { get; set; }

    public new DbSet<T> Set<T>() where T : class, IEntity
    {
        return base.Set<T>();
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
}
