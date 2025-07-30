using WebApi.BaseDatos;
using WebApi.Entidades;
using WebApi.Interfaces;

namespace WebApi.Repositories;

public class MaterialService : IMaterial
{
    private readonly ContextoBasedatos _context;

    public MaterialService(ContextoBasedatos context)
    {
        _context = context;
    }

    public List<Material> GetAll()
    {
        return _context.Materials.ToList();
    }
    public void Add(Material material)
    {
        _context.Materials.Add(material);
        _context.SaveChanges();
    }

    public void Update(Material material)
    {
        _context.Materials.Update(material);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var material = _context.Materials.Find(id);
        if (material != null)
        {
            _context.Materials.Remove(material);
            _context.SaveChanges();
        }
    }
}
