using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IBaseService<TDto,TDomain> where TDto : class where TDomain : class 
{
  Task<Pagination<TDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);
  Task<TDto> GetByGuidAsync(Guid guid, CancellationToken ct = default);
  // Task<Guid> CreateAsync(TCrDto dto, CancellationToken ct = default);
  // Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default);
  // Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
  // Task<Guid> UpdateAsync(TUpDto dto, CancellationToken ct = default);
  // Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default);
  // Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default);
  Task<IEnumerable<TDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default);
}
