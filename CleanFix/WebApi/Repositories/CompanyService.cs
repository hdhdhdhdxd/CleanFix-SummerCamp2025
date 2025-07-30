using WebApi.BaseDatos;
using WebApi.Entidades;
using WebApi.Interfaces;

namespace WebApi.Repositories;

public class CompanyService : ICompany
{
    private readonly ContextoBasedatos _context;

    public CompanyService(ContextoBasedatos context)
    {
        _context = context;
    }

    public List<Company> GetAll()
    {
        return _context.Companies.ToList();
    }
    public void Add(Company company)
    {
        _context.Companies.Add(company);
        _context.SaveChanges();
    }

    public void Update(Company company)
    {
        _context.Companies.Update(company);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var company = _context.Companies.Find(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
            _context.SaveChanges();
        }
    }
}
