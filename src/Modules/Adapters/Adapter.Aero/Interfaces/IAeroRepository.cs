using Adapter.Aero.Persistences.Entities;

namespace Adapter.Aero.Interfaces;

public interface IAeroRepository
{
      Task<ScpDeviceSpecification> GetScpDeviceSpecificationAsync(CancellationToken ct = default);
      Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationAsync(CancellationToken ct = default);
      Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationAsync(CancellationToken ct = default);
}