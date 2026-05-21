using SharedKernel.Domain;
using Time.Contract.DTOs;
using Time.Domain.Entities;

namespace Time.Application.Interfaces;

public interface ITimezoneRepository
{
      Task<short> GetLowestTimezoneComponentIdAsync(CancellationToken ct = default);
      Task<TimezoneDto> CreateAsync(Timezone timezone,CancellationToken ct = default);
      Task<TimezoneDto> DeleteByIdAsync(int id,CancellationToken ct = default);
      Task<TimezoneDto> GetByIdAsync(int id,CancellationToken ct = default);
      Task<Pagination<TimezoneDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
}