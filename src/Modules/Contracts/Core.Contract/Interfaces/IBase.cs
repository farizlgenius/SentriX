using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IBase<T, CR, UP> where T : class where CR : class where UP : class
{
  Task<Pagination<T>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);
  Task<T> GetByGuidAsync(Guid guid, CancellationToken ct = default);
  Task<Guid> CreateAsync(CR dto, CancellationToken ct = default);
  Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default);
  Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
  Task<Guid> UpdateAsync(UP dto, CancellationToken ct = default);
  Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default);
  Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default);
  Task<IEnumerable<T>> GetByLocationAsync(Guid guid, CancellationToken ct = default);
}
