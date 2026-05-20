using System;
using System.Reflection.PortableExecutable;
using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Domain.Entities;
using Device.Infrastructure.Persistences;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Infrastructure.Repositories;

public sealed class DeviceRepository(DeviceDbContext context) : IDeviceRepository
{
      public async Task<DeviceDto> CreateAsync(Domain.Entities.Devices domain, CancellationToken ct)
      {
            var device = new Persistences.Entities.Devices(domain);
            var data = await context.Devices.AddAsync(device, ct);
            var save = await context.SaveChangesAsync(ct);

            if (data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new DeviceDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.component_id,
                  data.Entity.serial_number,
                  data.Entity.mac,
                  data.Entity.ip,
                  data.Entity.port,
                  data.Entity.fw,
                  data.Entity.type,
                  data.Entity.status,
                  data.Entity.synced_at,
                  data.Entity.location_id,
                  data.Entity.metadata,
                  data.Entity.is_active);
      }


      public async Task<ModuleDto> CreateModuleAsync(Module dto, CancellationToken ct = default)
      {
            var data = await context.Modules.AddAsync(
                  new Persistences.Entities.Module(dto)
            );
            var save = await context.SaveChangesAsync(ct);

            if (data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            var module = await context.Modules.AsNoTracking().Include(x => x.devices).OrderByDescending(x => x.id).Where(x => x.id == data.Entity.id).FirstOrDefaultAsync();

            if (module == null)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new ModuleDto(
                  module.id,
                  module.component_id,
                  module.name,
                  module.fw,
                  module.serial_number,
                  module.port,
                 module.address,
                  module.mac,
                 module.model,
                 module.type,
                  module.devices.component_id,
                  module.location_id,
                  module.is_active
            );

      }


      public async Task<DeviceDto> GetByMacAsync(string Mac, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.mac.Equals(Mac))
            .Select(x => new DeviceDto(
                  x.id,
                  x.name,
                  x.component_id,
                  x.serial_number,
                  x.mac,
                  x.ip,
                  x.port,
                  x.fw,
                  x.type,
                  x.status,
                  x.synced_at,
                  x.location_id,
                  x.metadata,
                  x.is_active
                  )).FirstOrDefaultAsync() ?? new DeviceDto(
                        0,
                        string.Empty,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        DateTime.UtcNow,
                        0,
                        string.Empty,
                        false
                  );
      }

      public async Task<short> GetComponentIdByIdAsync(int id, CancellationToken ct = default)
      {
           return await context.Devices.AsNoTracking().OrderByDescending(x => x.id)
           .Where(x => x.id == id)
           .Select(x => x.component_id)
           .FirstOrDefaultAsync();
      }

      public async Task<int> GetComponentIdByMacAsync(string Mac, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.mac.Equals(Mac))
            .Select(x => x.component_id)
            .FirstOrDefaultAsync();
      }

      public async Task<DeviceDto> GetDeviceByComponentIdAsync(int ComponentId, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .Where(x => x.component_id == ComponentId)
            .OrderByDescending(x => x.id)
            .Select(x => new DeviceDto(
                  x.id,
                  x.name,
                  x.component_id,
                  x.serial_number,
                  x.mac,
                  x.ip,
                  x.port,
                  x.fw,
                  x.type,
                  x.status,
                  x.synced_at,
                  x.location_id,
                  x.metadata,
                  x.is_active
                  ))
            .FirstOrDefaultAsync() ?? new DeviceDto(
                  0,
                  string.Empty,
                  0,
                  string.Empty,
                  string.Empty,
                  string.Empty,
                  0,
                  string.Empty,
                  string.Empty,
                  string.Empty,
                  DateTime.UtcNow,
                  0,
                  string.Empty,
                  false
                  );
      }

      public async Task<int> GetIdByMacAsync(string Mac, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.Equals(Mac))
            .Select(x => x.id)
            .FirstOrDefaultAsync(ct);
      }

      public async Task<int> GetLowestModuleComponentIdByDeviceIdAsync(int device_id, CancellationToken ct = default)
      {
            return await ComponentHelper.LowestUnassignedNumberAsync<Device.Infrastructure.Persistences.Entities.Module>(
            context,
            x => x.device_id == device_id,
            x => x.component_id,
            ct);
      }

      public async Task<string> GetMacByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .Where(x => x.id == id)
            .OrderByDescending(x => x.id)
            .Select(x => x.mac)
            .FirstOrDefaultAsync(ct) ?? string.Empty;
      }

      public async Task<string> GetModelByModuleIdAsync(int ModuleId, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking().OrderByDescending(x => x.id).Where(x => x.id == ModuleId).Select(x => x.model).FirstOrDefaultAsync() ?? string.Empty;
      }

      public async Task<List<ModuleDto>> GetModuleByDeviceIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
                  .Where(m => m.device_id == id)
                  .Include(x => x.devices)
                  .Select(m => new ModuleDto(
                        m.id,
                        m.component_id,
                        m.name,
                        m.fw,
                        m.serial_number,
                        m.port,
                        m.address,
                        m.mac,
                        m.model,
                        m.type,
                        m.devices.component_id,
                        m.location_id,
                        m.is_active
                  )).ToListAsync(ct);
      }

      public async Task<ModuleDto> GetModuleByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.id == id)
            .Select(m => new ModuleDto(
                        m.id,
                        m.component_id,
                        m.name,
                        m.fw,
                        m.serial_number,
                        m.port,
                        m.address,
                        m.mac,
                        m.model,
                        m.type,
                        m.devices.component_id,
                        m.location_id,
                        m.is_active
                  ))
            .FirstOrDefaultAsync() ?? new ModuleDto(
                  0,
                  0, 
                  string.Empty,
                  string.Empty,
                  string.Empty,
                  0,
                  0,
                  string.Empty,
                  string.Empty,
                  string.Empty,
                  0,
                  0,
                  false);
      }

      public async Task<int> GetModuleIdByMacAndAddressAsync(string Mac, int Address, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.mac.Equals(Mac) && x.address == Address)
            .Select(x => x.id)
            .FirstOrDefaultAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceIdAsync(int DeviceId, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
                  .OrderByDescending(x => x.id)
                  .Where(x => x.device_id == DeviceId)
                  .Select(x => new OptionDto(x.name, x.id, string.Empty, false))
                  .ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByLocationIdAsync(int locationId, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.location_id == locationId)
            .Select(x => new OptionDto(x.name, x.id, string.Empty, false))
            .ToArrayAsync();
      }

      public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Devices.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.serial_number, pattern) ||
                                  EF.Functions.ILike(x.mac, pattern) ||
                                  EF.Functions.ILike(x.ip, pattern) ||
                                  EF.Functions.ILike(x.port.ToString(), pattern) ||
                                  EF.Functions.ILike(x.fw, pattern) ||
                                  EF.Functions.ILike(x.type, pattern) ||
                                  EF.Functions.ILike(x.status, pattern) ||
                              EF.Functions.ILike(x.metadata, pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.serial_number.Contains(search) ||
                                  x.mac.Contains(search) ||
                                  x.ip.Contains(search) ||
                                  x.port.ToString().Contains(search) ||
                                  x.fw.Contains(search) ||
                                  x.type.Contains(search) ||
                                  x.status.Contains(search) ||
                                  x.metadata.Contains(search)
                              );
                        }

                  }
            }

            if (param.locationId >= 0)
            {
                  query = query.Where(x => x.location_id == param.locationId || x.location_id == 1);
            }

            if (param.startDate != null)
            {
                  var startUtc = DateTime.SpecifyKind(param.startDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.created_at >= startUtc);
            }

            if (param.endDate != null)
            {
                  var endUtc = DateTime.SpecifyKind(param.endDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.created_at <= endUtc);
            }

            var count = await query.CountAsync();

            var res = await query.AsNoTracking()
            .OrderByDescending(e => e.created_at)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
            .Select(e => new DeviceDto(
                  e.id,
                  e.name,
                  e.component_id,
                  e.serial_number,
                  e.mac.Replace("_", ":"),
                  e.ip,
                  e.port,
                  e.fw,
                  e.type,
                  e.status,
                  e.synced_at,
                  e.location_id,
                  e.metadata,
                  e.is_active
            )).ToListAsync();

            return new Pagination<DeviceDto>(param.pageNumber, param.pageSize, count, (int)Math.Ceiling(count / (double)param.pageSize), res);
      }

      public async Task<bool> IsAnyModuleByIdAsync(int ModuleId, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking().AnyAsync(x => x.id == ModuleId);
      }

      public async Task<bool> IsAnyModuleBySerialNumberAsync(string SerialNumber, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking().AnyAsync(x => x.serial_number.Equals(SerialNumber), ct);
      }

      public async Task<bool> IsAnyWithMacAsync(string macAddress, CancellationToken ct)
      {
            return await context.Devices.AsNoTracking().AnyAsync(d => d.mac.Equals(macAddress), ct);
      }



      public async Task UpdateIpByMacAsync(int componentId, string ip, CancellationToken ct = default)
      {
            var entity = await context.Devices.FirstOrDefaultAsync(d => d.component_id == componentId, ct);
            if (entity is null)
                  return;

            entity.ip = ip;
            context.Devices.Update(entity);
            await context.SaveChangesAsync(ct);
      }

      public async Task UpdateModuleAsync(string Mac, int id, string SerialNumber, string Fw, short Port, CancellationToken ct = default)
      {
            var entity = await context.Modules.Where(x => x.id == id && x.devices.mac.Equals(Mac))
            .FirstOrDefaultAsync();

            if (entity == null)
                  return;

            entity.serial_number = SerialNumber;
            entity.fw = Fw;
            entity.port = Port;

            context.Modules.Update(entity);
            await context.SaveChangesAsync(ct);

      }

      public async Task UpdatePortByMacAsync(int componentId, int port, CancellationToken ct = default)
      {
            var entity = await context.Devices.FirstOrDefaultAsync(d => d.component_id == componentId, ct);
            if (entity is null)
                  return;

            entity.port = port;
            context.Devices.Update(entity);
            await context.SaveChangesAsync(ct);
      }

      public async Task VerifyDeviceMemoryAllocateStatusAsync(int componentId, string status, CancellationToken ct = default)
      {
            var entity = await context.Devices.FirstOrDefaultAsync(d => d.component_id == componentId, ct);
            if (entity is null)
                  return;

            entity.UpdateMemoryAllocateStatus(status);
            await context.SaveChangesAsync(ct);
      }
}
