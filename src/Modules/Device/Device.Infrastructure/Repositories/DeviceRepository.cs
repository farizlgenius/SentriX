using System;
using System.Reflection.PortableExecutable;
using Device.Application.Interfaces;
using Device.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Device.Infrastructure.Repositories;

public sealed class DeviceRepository(DeviceDbContext context) : IDeviceRepository
{
      public async Task<(string Mac, int LocationId)> GetDeviceMacAndLocationIdByComponentIdAsync(int componentId, CancellationToken ct)
      {
            var res = await context.devices.AsNoTracking()
                  .OrderByDescending(d => d.id)
                  .Where(d => d.component_id == componentId)
                  .FirstOrDefaultAsync(ct);

            return (res?.mac ?? string.Empty, res?.location_id ?? 0);

      }

      public async Task<string> GetDeviceMacByComponentIdAsync(int componentId, CancellationToken ct)
      {
            return await context.devices.AsNoTracking()
                  .Where(d => d.component_id == componentId)
                  .Select(d => d.mac)
                  .FirstOrDefaultAsync(ct) ?? string.Empty;
      }

      public async Task UpdateComponentIdAsync(int componentId, string mac, CancellationToken ct)
      {
            var entity = await context.devices.FirstOrDefaultAsync(d => d.mac == mac, ct);
            if(entity is null)
                  return;

            entity.component_id = componentId;
            await context.SaveChangesAsync(ct);
      }
}
