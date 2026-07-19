using Group.Contract.DTOs;
using Group.Domain.Entities;
using SharedKernel.Domain;

namespace Group.Application.Interfaces;

public interface IGroupRepository
{
      Task<short> GetLowestGroupComponentIdAsync(CancellationToken ct = default);
      Task CreateAsync(Groups dto,CancellationToken ct = default);
      Task UpdateAsync(Groups dto,CancellationToken ct = default);
      Task DeleteAsync(int id,CancellationToken ct = default);
      Task DeleteAsync(Guid guid,CancellationToken ct = default);
      Task<bool> IsAnyByIdAsync(int id,CancellationToken ct = default);
      Task<bool> IsAnyByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<GroupDto> GetByIdAsync(int id,CancellationToken ct = default);
      Task<GroupDto> GetByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<IEnumerable<GroupDto>> GetByLocationIdAsync(int location,CancellationToken ct = default);
      Task<IEnumerable<GroupSplitByMacDto>> GetByRangeGuidAsync(List<Guid> Guids,CancellationToken ct= default);
      Task<IEnumerable<GroupDto>> GetGroupByMacAsync(string Mac,string Type,CancellationToken ct = default);
      Task<IEnumerable<(Guid guid,short componentId)>> GetGroupGuidAndComponentIdsByMacAsync(string Mac,CancellationToken ct = default);
      Task<bool> IsAnyGroupNotSyncQueryAsync(int LocationId,DateTime SyncAt,CancellationToken ct = default);
      Task<IEnumerable<string>> MacsByGroupIdAsync(IEnumerable<int> Ids,CancellationToken ct = default);
}