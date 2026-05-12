using System;
using System.Reflection.PortableExecutable;
using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Helpers;

namespace Device.Infrastructure.Repositories;

public sealed class DeviceRepository(DeviceDbContext context) : IDeviceRepository
{
      public async Task<DeviceDto> CreateAsync(Domain.Entities.Devices domain, CancellationToken ct)
      {
            var device = new Persistences.Entities.Devices(domain);
            var data = await context.devices.AddAsync(device, ct);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new DeviceDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,data.Entity.serial_number,data.Entity.mac,data.Entity.ip,data.Entity.port,data.Entity.fw,data.Entity.type,data.Entity.status,data.Entity.synced_at,data.Entity.location_id,data.Entity.metadata);
      }

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

      public async Task<bool> IsAnyWithMacAsync(string macAddress, CancellationToken ct)
      {
            return await context.devices.AsNoTracking().AnyAsync(d => d.mac.Equals(macAddress), ct);
      }

      public async Task UpdateComponentIdAsync(int componentId, string mac, CancellationToken ct)
      {
            var entity = await context.devices.FirstOrDefaultAsync(d => d.mac == mac, ct);
            if(entity is null)
                  return;

            entity.component_id = componentId;
            await context.SaveChangesAsync(ct);
      }

      public async Task UpdateIpByMacAsync(string mac, string ip, CancellationToken ct = default)
      {
            var entity = await context.devices.FirstOrDefaultAsync(d => d.mac == mac, ct);
            if(entity is null)
                  return;

            entity.ip = ip;
            await context.SaveChangesAsync(ct);
      }

      public async Task UpdatePortByMacAsync(string mac, int port, CancellationToken ct = default)
      {
            var entity = await context.devices.FirstOrDefaultAsync(d => d.mac == mac, ct);
            if(entity is null)
                  return;

            entity.port = port;
            await context.SaveChangesAsync(ct);
      }

      public async Task VerifyDeviceMemoryAllocateStatusAsync(string mac, string status, CancellationToken ct = default)
      {
            var entity = await context.devices.FirstOrDefaultAsync(d => d.mac == mac, ct);
            if(entity is null)
                  return;

            entity.UpdateMemoryAllocateStatus(status);
            await context.SaveChangesAsync(ct);
      }
}
