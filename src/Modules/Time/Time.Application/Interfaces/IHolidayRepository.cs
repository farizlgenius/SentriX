using SharedKernel.Domain;
using Time.Contract.DTOs;
using Time.Domain.Entities;

namespace Time.Application.Interfaces;

public interface IHolidayRepository
{
      Task<HolidayDto> CreateHolidayAsync(Holiday domain,CancellationToken ct = default);
      Task<int> GetLowestHolidayComponentIdAsync(CancellationToken ct = default);
      Task<Pagination<HolidayDto>> HolidayPaginationAsync(PaginationParams param,CancellationToken ct = default);
}