

using Core.Domain.Entities;
using SharedKernel.Domain;


namespace Core.Application.Interfaces;

public interface IBaseRepository<TDto, TDomain> where TDto : class where TDomain : class
{
  Task<int> GetIdByGuidAsync(Guid guid,CancellationToken ct = default);
  Task<TDto> GetAsync(Guid guid, CancellationToken ct = default);
  Task AddAsync(TDomain entity, CancellationToken ct = default);
  Task UpdateAsync(TDomain entity, CancellationToken ct = default);
  Task DeleteAsync(Guid guid, CancellationToken ct = default);
  Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
  Task<bool> IsAnyByNameAndLocationGuidAsync(string name, Guid locationGuid = default, CancellationToken ct = default);
  Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default);
  Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default);
  Task<bool> EnableAsync(Guid guid, CancellationToken ct = default);
  Task<bool> DisableAsync(Guid guid, CancellationToken ct = default);
  Task<Pagination<TDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);

}