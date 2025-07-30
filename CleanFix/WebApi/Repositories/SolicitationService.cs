using WebApi.BaseDatos;
using WebApi.Entidades;
using WebApi.Interfaces;

namespace WebApi.Repositories;

public class SolicitationService : ISolicitation
{
    private readonly ContextoBasedatos _context;

    public SolicitationService(ContextoBasedatos context)
    {
        _context = context;
    }

    public List<Solicitation> GetAll()
    {
        return _context.Applications.ToList(); // DbSet<Solicitation> se llama Applications
    }

    public void Add(Solicitation solicitation)
    {
        _context.Applications.Add(solicitation);
        _context.SaveChanges();
    }

    public void Update(Solicitation solicitation)
    {
        _context.Applications.Update(solicitation);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var solicitud = _context.Applications.Find(id);
        if (solicitud != null)
        {
            _context.Applications.Remove(solicitud);
            _context.SaveChanges();
        }
    }
}
