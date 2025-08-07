using Infrastructure.Common.Interfaces;
using Infrastructure.Repositories;
using WebApi.Entidades;
using WebApi.Interfaces;
namespace WebApi.Services;

public class MaterialService : Repository<Material>, IMaterialRepository
{
    public MaterialService(IDatabaseContext database) : base(database)
    {
    }
}
