using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;
public interface IRepository<T>
{
    IQueryable<T> GetAll();
    Task<T?> GetByIdAsync(int id);
    int Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}
