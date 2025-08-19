using Application.Common.Interfaces;
using Infrastructure.Common.Interfaces;

namespace Infrastructure.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly IDatabaseContext _database;

    public UnitOfWork(IDatabaseContext database)
    {
        _database = database;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _database.SaveChangesAsync(cancellationToken);
    }
}
