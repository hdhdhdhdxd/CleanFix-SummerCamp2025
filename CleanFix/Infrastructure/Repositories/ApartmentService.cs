using Domain.Entities;
using Infrastructure.Common.Interfaces;
using Infrastructure.Repositories;
using WebApi.Interfaces;

namespace WebApi.Services;

public class ApartmentService : Repository<Apartment>, IApartmentRepository
{
    public ApartmentService(IDatabaseContext database) : base(database)
    {
    }
}
