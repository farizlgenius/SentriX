using SharedKernel.Domain;
using Time.Contract.DTOs;
using Time.Domain.Entities;

namespace Time.Application.Interfaces;

public interface IHolidayRepository
{
      Task AddAsync(Holiday domain,CancellationToken ct = default);
      Task<bool> IsAnyByGuidAsync(Guid guid,CancellationToken ct= default);
      Task<HolidayDto> GetByGuidAsync(Guid guid,CancellationToken ct = default);
      Task DeleteByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<Pagination<HolidayDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task UpdateAsync(Holiday domain,CancellationToken ct = default);
      Task<int> CountHolidayByLocationIdAsync(int location_id, CancellationToken ct = default);
}