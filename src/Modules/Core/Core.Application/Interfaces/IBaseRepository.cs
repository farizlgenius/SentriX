using System.Linq.Expressions;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface IBaseRepository<TEntity,TDomain,TDto> where TEntity : IBaseEntity where TDto : class where TDomain : BaseDomain
{
      Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default);
      Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default);
      Task<bool> EnableAsync(Guid guid, CancellationToken ct = default);
      Task<bool> IsEnabledAsync(Guid guid, CancellationToken ct = default);
      Task<bool> DisableAsync(Guid guid, CancellationToken ct = default);
      Task<bool> IsDisabledAsync(Guid guid, CancellationToken ct = default);
      Task DeleteAsync(Guid guid, CancellationToken ct = default);
      Task DeleteRangeAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
      Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default);
      Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<TDto> GetAsync(Guid guid,CancellationToken ct = default);
      Task AddAsync(TDomain domain, CancellationToken ct = default);
      Task UpdateAsync(TDomain domain,CancellationToken ct = default);
      Task<IEnumerable<TDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default);
      Task<string> GetNameByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<bool> IsAnyByNameAndLocationGuidAsync(string name, Guid locationGuid, CancellationToken ct = default);
      Task<Pagination<TDto>> GetPaginationAsync(
            PaginationParams param,
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, string>>[]? searchFields = null,
            CancellationToken ct = default
      );
}
