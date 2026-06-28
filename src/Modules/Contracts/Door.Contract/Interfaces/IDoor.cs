using Door.Contract.DTOs;
using SharedKernel.Domain;

namespace Door.Contract.Interfaces;

public interface IDoor
{
      Task<Pagination<DoorDto>> GetDoorPaginationAsync(PaginationParams param);
      Task<DoorDto> CreateAsync(CreateDoorDto dto);
      Task<DoorDto> UpdateAsync(DoorDto dto);
      Task<DoorDto> DeleteAsync(int id);
      Task<IEnumerable<OptionDto>> GetReaderModeAsync();
      Task<IEnumerable<OptionDto>> GetStrikeModeAsync();
      Task<IEnumerable<OptionDto>> GetApbModeAsync();
      Task<IEnumerable<OptionDto>> GetDoorModeAsync();
      Task<IEnumerable<OptionDto>> GetAccessControlFlagAsync();
      Task<IEnumerable<OptionDto>> GetSpareFlagAsync();
      Task<IEnumerable<OptionDto>> GetOsdpBaudrateAsync();
      Task<IEnumerable<OptionDto>> GetDoorOptionByLocationIdAsync(int LocationId);
      Task<string> GetNameByMacAndComponentIdAsync(string Mac,short ComponentId,CancellationToken ct=default);
}