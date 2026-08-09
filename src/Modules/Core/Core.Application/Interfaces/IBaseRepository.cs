using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface IBaseRepository<TDto,TDomain> where TDto : class where TDomain : class
{
  Task<TDto> GetAsync(Guid guid);
  Task<Pagination<TDto>> GetPaginationAsync(PaginationParams param);
  Task AddAsync(TDomain entity);
  Task UpdateAsync(TDomain entity);
  Task DeleteAsync(TDomain entity);
  
}