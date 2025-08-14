using Application.Common.Interfaces;

namespace Infrastructure.Common.Interfaces;
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
