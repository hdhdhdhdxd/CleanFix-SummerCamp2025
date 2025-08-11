using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common.Interfaces;
namespace Infrastructure.Repositories;

public class CompanyService : Repository<Company>, ICompanyRepository
{

    public CompanyService(IDatabaseContext database) : base(database)
    {
    }
}
