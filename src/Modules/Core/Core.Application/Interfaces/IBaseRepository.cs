namespace Core.Application.Interfaces;

public interface IBaseRepository<T> where T : class
{
  Task<T?> GetAsync(Guid guid);
  Task<IEnumerable<T>> ListAsync();
  Task AddAsync(T entity);
  Task UpdateAsync(T entity);
  Task DeleteAsync(T entity);
}