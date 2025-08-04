using System;
using WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApi.BaseDatos;
using WebApi.Entidades;
namespace WebApi.Services;

public class ApartmentService : IApartment
{
    private readonly ContextoBasedatos _context;

    public ApartmentService(ContextoBasedatos context)
    {
        _context = context;
    }

    public List<Apartment> GetAll()
    {
        return _context.Apartments.ToList(); // O usa async si prefieres
    }

    public void Add(Apartment apartment)
    {
        _context.Apartments.Add(apartment);
        _context.SaveChanges();
    }

    public void Update(Apartment apartment)
    {
        _context.Apartments.Update(apartment);
        _context.SaveChanges();
    }

    public void Delete(int apartmentId)
    {
        var apartment = _context.Apartments.Find(apartmentId);
        if (apartment != null)
        {
            _context.Apartments.Remove(apartment);
            _context.SaveChanges();
        }
    }
}
