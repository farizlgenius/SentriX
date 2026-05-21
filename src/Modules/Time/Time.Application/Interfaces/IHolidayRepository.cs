using SharedKernel.Domain;
using Time.Contract.DTOs;
using Time.Domain.Entities;

namespace Time.Application.Interfaces;

public interface IHolidayRepository
{
      Task<HolidayDto> CreateHolidayAsync(Holiday domain,CancellationToken ct = default);
      Task<int> GetLowestHolidayComponentIdAsync(CancellationToken ct = default);
      Task<HolidayDto> GetByIdAsync(int id,CancellationToken ct = default);
      Task<HolidayDto> DeleteByIdAsync(int id,CancellationToken ct = default);
      Task<Pagination<HolidayDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
}