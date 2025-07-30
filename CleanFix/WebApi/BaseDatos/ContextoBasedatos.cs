using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;

namespace WebApi.BaseDatos;

public class ContextoBasedatos: DbContext
{
    public ContextoBasedatos(DbContextOptions<ContextoBasedatos> options) : base(options)
    {

    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Solicitation> Applications { get; set; }
}
