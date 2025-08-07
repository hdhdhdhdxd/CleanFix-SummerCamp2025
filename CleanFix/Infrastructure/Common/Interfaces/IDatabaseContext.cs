using System.Collections.Generic;
using System.Security.Principal;
using Dominio.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;

namespace Infrastructure.Common.Interfaces;
public interface IDatabaseContext
{
    DbSet<Apartment> Apartments { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Solicitation> Solicitations { get; set; }
    DbSet<Company> Companies { get; set; }
    DbSet<Material> Materials { get; set; }

    DbSet<T> Set<T>() where T : class, IEntity;
}
