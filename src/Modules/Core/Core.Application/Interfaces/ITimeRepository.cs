using Core.Contract.DTOs.Time;

namespace Core.Application.Interfaces;

public interface ITimeRepository : IBaseRepository<TimeZoneDto, Domain.Entities.TimeZone>
{
    Task<Dictionary<Guid, int>> GetTimeZoneIdsMapGuidsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
}