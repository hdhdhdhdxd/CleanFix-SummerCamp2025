using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common.Interfaces;
using Infrastructure.Repositories;

namespace WebApi.Services;

public class SolicitationService : Repository<Solicitation>, ISolicitationRepository
{
    public SolicitationService(IDatabaseContext database) : base(database)
    {
    }
}
