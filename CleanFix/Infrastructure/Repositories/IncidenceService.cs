using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common.Interfaces;
using Infrastructure.Repositories;

namespace WebApi.Services;

public class IncidenceService : Repository<Incidence>, IIncidenceRepository
{
    public IncidenceService(IDatabaseContext database) : base(database)
    {
    }
}
