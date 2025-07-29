using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;

namespace WebApi.BaseDatos;

public class ContextoBasedatos: DbContext
{
    public ContextoBasedatos(DbContextOptions<ContextoBasedatos> options) : base(options)
    {

    }

    public DbSet<Empresa> Empresas { get; set; }
}
