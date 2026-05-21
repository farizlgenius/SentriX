using Adapter.Aero.Persistences.Entities;
using SharedKernel.Domain;

namespace Adapter.Aero.Interfaces;

public interface IAeroRepository
{
      Task<ScpDeviceSpecification> GetScpDeviceSpecificationAsync(CancellationToken ct = default);
      Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationAsync(CancellationToken ct = default);
      Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetRelayOptionAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetTimezoneModeAsync(CancellationToken ct = default);
}