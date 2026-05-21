using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

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

      public async Task<IEnumerable<OptionDto>> GetRelayOptionAsync(CancellationToken ct = default)
      {
            return await context.RelayModes.AsNoTracking().Select(x => new OptionDto(
                  x.label,
                  x.value,
                  string.Empty,
                  0,
                  false
            )).ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetTimezoneModeAsync(CancellationToken ct = default)
      {
            return await context.TimezoneModes.AsNoTracking().Select(x => new OptionDto(
                  x.label,
                  x.value,
                  x.description,
                  0,
                  false
            )).ToArrayAsync();
      }

      public async Task<ScpDeviceSpecification> GetScpDeviceSpecificationAsync(CancellationToken ct = default)
      {
            return await context.ScpDeviceSpecifications.AsNoTracking()
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync() ?? new ScpDeviceSpecification();
      }
}