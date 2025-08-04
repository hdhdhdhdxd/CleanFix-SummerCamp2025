using WebApi.BaseDatos;
using WebApi.Entidades;
using WebApi.Interfaces;
namespace WebApi.Services;

public class SolicitationService : ISolicitation
{
    private readonly ContextoBasedatos _context;

    public SolicitationService(ContextoBasedatos context)
    {
        _context = context;
    }

    public List<Solicitation> GetAll()
    {
        return _context.Solicitations.ToList(); // DbSet<Solicitation> se llama Applications
    }

    public void Add(Solicitation solicitation)
    {
        _context.Solicitations.Add(solicitation);
        _context.SaveChanges();
    }

    public void Update(Solicitation solicitation)
    {
        _context.Solicitations.Update(solicitation);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var solicitud = _context.Solicitations.Find(id);
        if (solicitud != null)
        {
            _context.Solicitations.Remove(solicitud);
            _context.SaveChanges();
        }
    }
}
