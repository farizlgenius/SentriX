using Core.Contract.DTOs.Time;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface ITime : IBase<TimeZoneDto, CreateTimeZoneDto, UpdateTimeZoneDto>
{
      Task UploadAsync(Guid locationGuid,CancellationToken ct = default);
}