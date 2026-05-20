using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Aero.Repositories;

public sealed class AeroRepository(AeroDbContext context) : IAeroRepository
{
      public async Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationAsync(CancellationToken ct = default)
      {
            return await context.AccessDatabaseSpecifications.AsNoTracking()
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync() ?? new AccessDatabaseSpecification();
      }

      public async Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationAsync(CancellationToken ct = default)
      {
            return await context.ElevatorAccessLevelSpecifications.AsNoTracking()
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync() ?? new ElevatorAccessLevelSpecification();
      }

      public async Task<ScpDeviceSpecification> GetScpDeviceSpecificationAsync(CancellationToken ct = default)
      {
            return await context.ScpDeviceSpecifications.AsNoTracking()
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync() ?? new ScpDeviceSpecification();
      }
}