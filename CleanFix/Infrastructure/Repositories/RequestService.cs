using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common.Interfaces;
using Infrastructure.Repositories;

namespace WebApi.Services;

public class RequestService : Repository<Request>, IRequestRepository
{
    public RequestService(IDatabaseContext database) : base(database)
    {
    }
}
