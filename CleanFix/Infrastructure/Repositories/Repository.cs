using System.Security.Principal;
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

    public IQueryable<T> GetAll()
    {
        return _database.Set<T>();
    }

    public T Get(Guid id)
    {
        return _database.Set<T>()
            .Single(p => p.Id == id);
    }

    public void Add(T entity)
    {
        _database.Set<T>().Add(entity);
    }

    public async Task Remove(T entity)
    {
        await _database.Set<T>().Where(e => e.Id == entity.Id).ExecuteDeleteAsync();
    }
}
