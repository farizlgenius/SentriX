using SharedKernel.Domain;
using Time.Contract.DTOs;

namespace Time.Contract.Interfaces;

public interface ITime
{
      Task<HolidayDto> CreateHolidayAsync(CreateHolidayDto dto);
      Task<Pagination<HolidayDto>> HolidayPaginationAsync(PaginationParams param);
}