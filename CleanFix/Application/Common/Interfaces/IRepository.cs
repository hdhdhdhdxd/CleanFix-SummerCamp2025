using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;
public interface IRepository<T>
{
    IQueryable<T> GetAll();
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task RemoveAsync(T entity, CancellationToken cancellationToken);
}
