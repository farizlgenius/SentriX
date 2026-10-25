

using Core.Contract.DTOs;
using Core.Contract.DTOs.DeviceModule;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;


namespace Core.Application.Interfaces;

public interface IRepository<TDto, TDomain> where TDto : class where TDomain : class
{
  Task<(int,short)> GetIdAndExternalIdByGuidAsync(Guid guid,CancellationToken ct = default);
  Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default);
  Task<Guid> GetGuidByIdAsync(int id,CancellationToken ct = default);
  Task<TDto> GetAsync(Guid guid, CancellationToken ct = default);
  Task AddAsync(TDomain entity, CancellationToken ct = default);
  Task UpdateAsync(TDomain entity, CancellationToken ct = default);
  Task DeleteAsync(Guid guid, CancellationToken ct = default);
  Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
  Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default);
  Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default);
  Task<bool> EnableAsync(Guid guid, CancellationToken ct = default);
  Task<bool> IsEnabledAsync(Guid guid,CancellationToken ct = default);
  Task<bool> DisableAsync(Guid guid, CancellationToken ct = default);
  Task<bool> IsDisabledAsync(Guid guid,CancellationToken ct = default);
  Task<Pagination<TDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default);
  Task<IEnumerable<TDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default);
  Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default);
  Task<string> GetNameByGuidAsync(Guid guid,CancellationToken ct = default);
  Task<bool> IsAnyByNameAndLocationGuidAsync(string name,Guid guid,CancellationToken ct = default);
  Task<bool> IsAnyByNameAsync(string name,CancellationToken ct = default);
  Task<short> GetFreeExeternalIdAsync(int max,IEnumerable<short>? exception = default,CancellationToken ct = default);
  
  Task<CommandDetailDto> GetCommandDetailAsync(Guid guid,CancellationToken ct = default);
  Task<short> GetExternalIdByGuidAsync(Guid guid, CancellationToken ct = default);
  Task<IEnumerable<short>> GetRangeExernalIdByGuidsAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
  Task<Dictionary<Guid,short>> GetExternalIdMapGuidByGuidRangeAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
  
  

}