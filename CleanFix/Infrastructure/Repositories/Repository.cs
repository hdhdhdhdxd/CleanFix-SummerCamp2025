using Application.Common.Interfaces;
using Dominio.Common.Interfaces;
using Infrastructure.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class Repository<T>
       : IRepository<T>
       where T : class, IEntity

{
    private readonly IDatabaseContext _database;

    public Repository(IDatabaseContext database)
    {
        _database = database;
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _database.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _database.Set<T>().SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Guid> AddAsync(T entity, CancellationToken cancellationToken)
    {
        _database.Set<T>().Add(entity);

        await _database.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _database.Set<T>().Update(entity);
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(T entity, CancellationToken cancellationToken)
    {
        await _database.Set<T>().Where(e => e.Id == entity.Id).ExecuteDeleteAsync();

        await _database.SaveChangesAsync(cancellationToken);
    }
}
