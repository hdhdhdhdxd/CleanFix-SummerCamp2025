using Domain.Entities;
using Infrastructure.Common.Interfaces;
using WebApi.Interfaces;
namespace Infrastructure.Repositories;

public class CompanyService : Repository<Company>, ICompanyRepository
{

    public CompanyService(IDatabaseContext database) : base(database)
    {
    }
}
