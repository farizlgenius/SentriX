using Core.Contract.DTOs.Device;
using Core.Contract.DTOs.Time;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface ITime : IBase<TimeZoneDto, CreateTimeZoneDto, UpdateTimeZoneDto>
{
      Task UploadAsync(Guid locationGuid,CancellationToken ct = default);
      Task<Components> GetComponentsAsync(Guid locationGuid,DateTime syncedAt,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOptionByLocationAsync(Guid guid,CancellationToken ct = default);
}