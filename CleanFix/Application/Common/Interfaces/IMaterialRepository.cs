using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;
public interface IMaterialRepository
{
    Task<List<Material>> GetAllAsync(CancellationToken cancellationToken);
    Task<Material?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> AddAsync(Material entity, CancellationToken cancellationToken);
    Task UpdateAsync(Material entity, CancellationToken cancellationToken);
    Task RemoveAsync(Material entity, CancellationToken cancellationToken);
}
