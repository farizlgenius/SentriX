using SharedKernel.Domain;
using Time.Contract.DTOs;

namespace Time.Contract.Interfaces;

public interface ITime
{
      Task<HolidayDto> CreateHolidayAsync(CreateHolidayDto dto);
      Task<Pagination<HolidayDto>> HolidayPaginationAsync(PaginationParams param);
      Task<HolidayDto> DeleteHolidayAsync(int id);
      Task<HolidayDto> UpdateHolidayAsync(HolidayDto dto);

      // Timezone
      Task<Pagination<TimezoneDto>> TimezonePaginationAsync(PaginationParams param);
      Task<TimezoneDto> CreateTimezoneAsync(CreateTimezoneDto dto);
      Task<TimezoneDto> UpdateTimezoneAsync(TimezoneDto dto);
      Task<TimezoneDto> DeleteTimezoneAsync(int id);
      Task<IEnumerable<OptionDto>> GetTimezoneModeAsync(string Type);
}