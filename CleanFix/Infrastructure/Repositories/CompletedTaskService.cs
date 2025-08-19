using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common.Interfaces;
using Infrastructure.Repositories;
namespace WebApi.Services;

public class CompletedTaskService : Repository<CompletedTask>, ICompletedTaskRepository
{
    public CompletedTaskService(IDatabaseContext database) : base(database)
    {
    }
}
