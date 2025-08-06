using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Entidades;

namespace WebApi.Interfaces;

public interface IApartment
{
    public Task<List<Apartment>> GetAll();
    public void Add(Apartment apartment);
    public void Update(Apartment apartment);
    public void Delete(int apartmentId);
}

