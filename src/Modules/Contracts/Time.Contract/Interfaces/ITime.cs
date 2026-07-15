using SharedKernel.Domain;
using Time.Contract.DTOs;

namespace Time.Contract.Interfaces;

public interface ITime
{
      Task<HolidayDto> CreateHolidayAsync(CreateHolidayDto dto);
      Task<Pagination<HolidayDto>> HolidayPaginationAsync(PaginationParams param);
      Task<HolidayDto> DeleteHolidayByGuidAsync(Guid guid);
      Task<HolidayDto> UpdateHolidayAsync(HolidayDto dto);

      // Timezone
      Task<Pagination<TimeZoneDto>> TimezonePaginationAsync(PaginationParams param);
      Task<IEnumerable<OptionDto>> GetTimezoneOptionByLocationIdAsync(int locationId);
      Task<TimeZoneDto> CreateTimezoneAsync(CreateTimezoneDto dto);
      Task<TimeZoneDto> UpdateTimezoneAsync(TimeZoneDto dto);
      Task<TimeZoneDto> DeleteTimeZoneByGuidAsync(Guid guid);
      Task<IEnumerable<OptionDto>> GetTimezoneModeAsync(string Type);
}