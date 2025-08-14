using Application.Common.Interfaces;
using Domain.Common.Interfaces;
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

    public IQueryable<T> GetAll()
    {
        return _database.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _database.Set<T>().FirstOrDefaultAsync(p => p.Id == id);
    }
            
    public void Add(T entity)
    {
        _database.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _database.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        _database.Set<T>().Remove(entity);
    }
}
