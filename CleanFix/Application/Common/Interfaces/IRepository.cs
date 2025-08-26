namespace Application.Common.Interfaces;
public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();
    IQueryable<T> GetQueryable();
    Task<T?> GetByIdAsync(int id);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}
