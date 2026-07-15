using SharedKernel.Domain;
using Time.Contract.DTOs;
using Time.Domain.Entities;

namespace Time.Application.Interfaces;

public interface ITimeZoneRepository
{
      Task<short> GetLowestTimeZoneComponentIdAsync(int location_id,CancellationToken ct = default);
      Task AddAsync(Domain.Entities.TimeZone timezone,CancellationToken ct = default);
      Task UpdateAsync(Domain.Entities.TimeZone timezone,CancellationToken ct = default);
      Task DeleteByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<TimeZoneDto> GetByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<Pagination<TimeZoneDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetTimezoneOptionByLocationIdAsync(int locationId,CancellationToken ct = default);
      Task<IEnumerable<TimeZoneDto>> GetTimeZoneByLocationIdAsync(int locationId,CancellationToken ct = default);
      Task<bool> IsAnyTimeZoneNotSyncAsync(int LocationId,DateTime SyncAt,CancellationToken ct = default);
      Task<bool> IsAnyNameAsync(string name,CancellationToken ct = default);
      Task<short> GetLowestIntervalComponentIdExceptStartFromOneAsync(
            List<short> Excepts,
            Guid TzGuid,
            CancellationToken ct = default
            );

      Task<int> CountTimeZoneByLocationIdAsync(int location_id,CancellationToken ct = default);

      
}