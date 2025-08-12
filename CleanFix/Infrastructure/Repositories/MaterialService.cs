using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common.Interfaces;
using Infrastructure.Repositories;
namespace WebApi.Services;

public class MaterialService : Repository<Material>, IMaterialRepository
{
    public MaterialService(IDatabaseContext database) : base(database)
    {
    }
}
