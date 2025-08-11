using Domain.Entities;
using Infrastructure.Common.Interfaces;
using Infrastructure.Repositories;
using WebApi.Interfaces;
namespace WebApi.Services;

public class SolicitationService : Repository<Solicitation>, ISolicitationRepository
{
    public SolicitationService(IDatabaseContext database) : base(database)
    {
    }
}

