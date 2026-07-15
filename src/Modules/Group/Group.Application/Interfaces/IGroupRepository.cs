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
      Task<bool> IsAnyByIdAsync(int id,CancellationToken ct = default);
      Task<GroupDto> GetByIdAsync(int id,CancellationToken ct = default);
      Task<GroupDto> GetByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<IEnumerable<GroupDto>> GetByLocationIdAsync(int location,CancellationToken ct = default);
      Task<IEnumerable<GroupSplitByMacDto>> GetByRangeIdAsync(List<int> Ids,CancellationToken ct= default);
      Task<IEnumerable<GroupDto>> GetGroupByMacAsync(string Mac,string Type,CancellationToken ct = default);
      Task<IEnumerable<(int id,short componentId)>> GetGroupIdAndComponentIdListByMacAsync(string Mac,CancellationToken ct = default);
      Task<bool> IsAnyGroupNotSyncQueryAsync(int LocationId,DateTime SyncAt,CancellationToken ct = default);
      Task<IEnumerable<string>> MacsByGroupIdAsync(IEnumerable<int> Ids,CancellationToken ct = default);
}