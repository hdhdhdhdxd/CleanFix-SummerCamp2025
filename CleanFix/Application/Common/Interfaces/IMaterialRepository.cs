using Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;
public interface IMaterialRepository
{
    IQueryable<Material> GetAll();
    Task<Material?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> AddAsync(Material entity, CancellationToken cancellationToken);
    Task UpdateAsync(Material entity, CancellationToken cancellationToken);
    Task RemoveAsync(Material entity, CancellationToken cancellationToken);
}
