using System.Collections.Generic;
using System.Security.Principal;
using Dominio.Common.Interfaces;
using Dominio.Maintenance;
using Microsoft.EntityFrameworkCore;

namespace Infrastrucure.Common.Interfaces;
public interface IDatabaseContext
{
    DbSet<Apartment> Apartments { get; set; }

    DbSet<T> Set<T>() where T : class, IEntity;
}
