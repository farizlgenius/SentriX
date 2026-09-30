using Core.Contract.DTOs.Device;
using Core.Contract.DTOs.Time;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface ITimeRepository : IBaseRepository<TimeZoneDto, Domain.Entities.TimeZone>
{
    Task<Dictionary<Guid, int>> GetTimeZoneIdsMapGuidsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
    Task<Components> GetComponentsAsync(Guid locationGuid, DateTime syncedAt, CancellationToken ct = default);
    Task<IEnumerable<OptionDto>> GetOptionByLocationAsync(Guid guid,CancellationToken ct = default);
}