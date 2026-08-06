using Door.Contract.DTOs;
using Door.Domain.Entities;
using SharedKernel.Domain;

namespace Door.Application.Interfaces;

public interface IDoorRepository
{
      Task AddAsync(Doors domain,CancellationToken ct = default);
      Task<bool> IsAnyByIdAsync(int id,CancellationToken ct = default);
      Task<bool> IsAnyByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<DoorDto> GetByIdAsync(int id,CancellationToken ct = default);
      Task<DoorDto> GetByGuidAsync(Guid guid,CancellationToken ct = default);
      Task DeleteAsync(Guid guid,CancellationToken ct = default);
      Task UpdateAsync(Doors domain,CancellationToken ct = default);
      Task<Pagination<DoorDto>> GetDoorPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetReaderModeAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetStrikeModeAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetApbModeAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetDoorModeAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetAccessControlFlagAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetSpareFlagAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOsdpBaudrateAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetDoorOptionByLocationIdAsync(int LocationId,CancellationToken ct= default);
      Task<string> GetNameByMacAndComponentIdAsync(string Mac,short ComponentId,CancellationToken ct= default);
      Task<IEnumerable<DoorDto>> GetDoorByMacAsync(string Mac,CancellationToken ct = default);
      Task<bool> IsAnyDoorNotSyncAsync(string Mac,int LocationId,DateTime SyncAt,CancellationToken ct = default);
      
      
}