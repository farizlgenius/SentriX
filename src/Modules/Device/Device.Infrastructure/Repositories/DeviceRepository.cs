using System;
using System.Reflection.PortableExecutable;
using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Domain.Entities;
using Device.Infrastructure.Persistences;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Helpers;

namespace Device.Infrastructure.Repositories;

public sealed class DeviceRepository(DeviceDbContext context) : IDeviceRepository
{
      public async Task<bool> AddReaderAsync(Domain.Entities.Reader domain, CancellationToken ct = default)
      {
            var data = await context.Readers.AddAsync(new Persistences.Entities.Reader(domain));
            var save = await context.SaveChangesAsync();

            if (data.Entity is null || save <= 0)
                  return false;

            return true;

      }

            public async Task<bool> AddInputAsync(Domain.Entities.Input domain, CancellationToken ct = default)
      {
            var data = await context.Inputs.AddAsync(new Persistences.Entities.Input(domain));
            var save = await context.SaveChangesAsync();

            if (data.Entity is null || save <= 0)
                  return false;

            return true;

      }

            public async Task<bool> AddRelayAsync(Domain.Entities.Relay domain, CancellationToken ct = default)
      {
            var data = await context.Relays.AddAsync(new Persistences.Entities.Relay(domain));
            var save = await context.SaveChangesAsync();

            if (data.Entity is null || save <= 0)
                  return false;

            return true;

      }

      public async Task AddAsync(Domain.Entities.Devices domain, CancellationToken ct)
      {
            var device = new Persistences.Entities.Devices(domain);
            await context.Devices.AddAsync(device, ct);
            await context.SaveChangesAsync(ct);

      }


      public async Task AddModuleAsync(Module dto, CancellationToken ct = default)
      {
            await context.Modules.AddAsync(
                  new Persistences.Entities.Module(dto)
            );
            await context.SaveChangesAsync(ct);

      }


      public async Task<DeviceDto> GetByMacAsync(string Mac, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.mac.Equals(Mac))
            .Select(x => new DeviceDto(
                  x.guid,
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
                  );
      }

      public async Task<string> GetMacByGuidAsync(Guid guid, CancellationToken ct = default)
      {
           return await context.Devices.AsNoTracking().OrderByDescending(x => x.id)
           .Where(x => x.guid == guid)
           .Select(x => x.mac)
           .FirstOrDefaultAsync() ?? string.Empty;
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
                   x.guid,
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
            .FirstOrDefaultAsync() ?? new DeviceDto();
      }

      public async Task<int> GetIdByMacAsync(string Mac, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.Equals(Mac))
            .Select(x => x.id)
            .FirstOrDefaultAsync(ct);
      }

      public async Task<int> GetLowestModuleComponentIdByDeviceGuidAsync(Guid device_guid, CancellationToken ct = default)
      {
            return await ComponentHelper.LowestUnassignedNumberAsync<Device.Infrastructure.Persistences.Entities.Module>(
            context,
            x => x.device_guid == device_guid,
            x => x.component_id,
            ct);
      }

      public async Task<string> GetMacByComponentIdAsync(int ComponentId)
      {
            return await context.Devices.AsNoTracking().Where(x => x.component_id == ComponentId).Select(x => x.mac).FirstOrDefaultAsync() ?? string.Empty;
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

      public async Task<IEnumerable<ModuleDto>> GetModuleByDeviceGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
                  .Where(m => m.device_guid == guid)
                  .Include(x => x.devices)
                  .Select(m => new ModuleDto(
                        m.guid,
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
                  )).ToArrayAsync(ct);
      }

      public async Task<ModuleDto> GetModuleByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .Select(m => new ModuleDto(
                        m.guid,
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
            .FirstOrDefaultAsync() ?? new ModuleDto();
      }

      public async Task<int> GetModuleIdByMacAndAddressAsync(string Mac, int Address, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.mac.Equals(Mac) && x.address == Address)
            .Select(x => x.id)
            .FirstOrDefaultAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceGuidAsync(Guid DeviceGuid, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
                  .OrderByDescending(x => x.id)
                  .Where(x => x.device_guid == DeviceGuid)
                  .Select(x => new OptionDto(x.name, x.component_id,x.mac,x.guid, false))
                  .ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByLocationIdTypeAeroAsync(int locationId,string type, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.location_id == locationId && x.type == DeviceType.aero.ToString())
            .Select(x => new OptionDto(x.name, x.component_id,x.mac,x.guid,false))
            .ToArrayAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByLocationIdTypeAmicoAsync(int locationId,string type, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.location_id == locationId && x.type == DeviceType.amico.ToString())
            .Select(x => new OptionDto(x.name, x.component_id,x.mac,x.guid,false))
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
                  e.guid,
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

      public async Task<IEnumerable<(string Mac, short ComponentId,string Type)>> MacAndComponentIdListAsync(int LocationId,CancellationToken ct = default)
      {
            var list = await context.Devices.AsNoTracking()
            .Where(x => x.location_id == LocationId)
                  .Select(x => new { x.mac, x.component_id,x.type })
                  .ToListAsync(ct);

            return list.Select(x => (x.mac, x.component_id,x.type));

      }

      public async Task UpdateIpByComponentIdAsync(int componentId, string ip, CancellationToken ct = default)
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

      public async Task UpdatePortByComponentIdAsync(int componentId, int port, CancellationToken ct = default)
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

            entity.UpdateSyncStatus(status);
            await context.SaveChangesAsync(ct);
      }

      public async Task<IEnumerable<OptionDto>> GetReaderOptionsByModuleGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var data = await context.Modules.AsNoTracking()
            .Where(x => x.guid == guid)
            .OrderByDescending(x => x.id)
            .Select(x => new {x.guid,x.mac,x.model})
            .FirstOrDefaultAsync();

            if(data == null)
                  return new List<OptionDto>();

            if (Enum.TryParse<SioModel>(data.model, out var sioModel))
            {
                  var res = new List<OptionDto>();
                  int value = (int)sioModel;
                  int max = AeroModuleModelHelper.nReaderByModel((SioModel)value);

                  IEnumerable<int> unavailable = await context.Readers.AsNoTracking()
                  .Where(x => x.module.mac == data.mac && x.module_guid == data.guid)
                  .Select(x => x.reader_number)
                  .ToArrayAsync();

                  int[] arr = Enumerable.Range(0, max).ToArray();
                  int[] available = arr.Except(unavailable).ToArray();
                  foreach(int a in available)
                  {
                        res.Add(
                              new OptionDto(
                                    $"Reader {a + 1}",
                                    a,
                                    string.Empty,
                                    default,
                                    false
                                    )
                        );
                  }
                  return res;
            }
            else
            {
                  return new List<OptionDto>();
            }

            
      }

      public async Task<IEnumerable<OptionDto>> GetRelayOptionsByModuleIdAsync(Guid guid, CancellationToken ct = default)
      {
            var data = await context.Modules.AsNoTracking()
            .Where(x => x.guid == guid)
            .OrderByDescending(x => x.id)
            .Select(x => new {x.guid,x.mac,x.model})
            .FirstOrDefaultAsync();

            if(data == null)
                  return new List<OptionDto>();

            if (Enum.TryParse<SioModel>(data.model, out var sioModel))
            {
                  var res = new List<OptionDto>();
                  int value = (int)sioModel;
                  int max = AeroModuleModelHelper.nReaderByModel((SioModel)value);

                  IEnumerable<int> unavailable = await context.Relays.AsNoTracking()
                  .Where(x =>  x.module.mac == data.mac && x.module_guid == data.guid)
                  .Select(x => x.relay_number)
                  .ToArrayAsync();

                  int[] arr = Enumerable.Range(0, max).ToArray();
                  int[] available = arr.Except(unavailable).ToArray();
                  foreach(int a in available)
                  {
                        res.Add(
                              new OptionDto(
                                    $"Relay {a + 1}",
                                    a,
                                    string.Empty,
                                    default,
                                    false
                                    )
                        );
                  }
                  return res;
            }
            else
            {
                  return new List<OptionDto>();
            }

      }

      public async Task<IEnumerable<OptionDto>> GetInputOptionsByModuleIdAsync(Guid guid, CancellationToken ct = default)
      {
            var data = await context.Modules.AsNoTracking()
            .Where(x => x.guid == guid)
            .OrderByDescending(x => x.id)
            .Select(x => new {x.guid,x.mac,x.model})
            .FirstOrDefaultAsync();

            if(data == null)
                  return new List<OptionDto>();

            if (Enum.TryParse<SioModel>(data.model, out var sioModel))
            {
                  var res = new List<OptionDto>();
                  int value = (int)sioModel;
                  int max = AeroModuleModelHelper.nReaderByModel((SioModel)value);

                  IEnumerable<int> unavailable = await context.Inputs.AsNoTracking()
                  .Where(x =>  x.module.mac == data.mac && x.module_guid == data.guid)
                  .Select(x => x.input_number)
                  .ToArrayAsync();

                  int[] arr = Enumerable.Range(0, max).ToArray();
                  int[] available = arr.Except(unavailable).ToArray();
                  foreach(int a in available)
                  {
                        res.Add(
                              new OptionDto(
                                    $"Input {a + 1}",
                                    a,
                                    string.Empty,
                                    default,
                                    false
                                    )
                        );
                  }
                  return res;
            }
            else
            {
                  return new List<OptionDto>();
            }

      }

      public async Task<int> GetIdByComponentIdAsync(short ComponentId, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .Where(x => x.component_id == ComponentId)
            .OrderByDescending(x => x.id)
            .Select(x => x.id)
            .FirstOrDefaultAsync();
      }

      public async Task<string> GetModuleNameByMacAndComponentIdAsync(string Mac, short ComponentId, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
            .Where(x => x.mac.Equals(Mac) && x.component_id == ComponentId)
            .OrderByDescending(x => x.id)
            .Select(x => x.name)
            .FirstOrDefaultAsync() ?? string.Empty; 
      }

      public async Task<bool> DeleteReaderAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Readers
            .Where(x => x.guid == guid)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync();

            if(entity == null)
                  return false;

            var data = context.Readers.Remove(entity);
            var save = await context.SaveChangesAsync();

            if(data.Entity == null || save <= 0)
                  return false;

            return true;
      }

      public async Task<bool> DeleteInputAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Inputs
            .Where(x => x.guid == guid)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync();

            if(entity == null)
                  return false;

            var data = context.Inputs.Remove(entity);
            var save = await context.SaveChangesAsync();

            if(data.Entity == null || save <= 0)
                  return false;

            return true;
      }

      public async Task<bool> DeleteRelayAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Relays
            .Where(x => x.guid == guid)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync();

            if(entity == null)
                  return false;

            var data = context.Relays.Remove(entity);
            var save = await context.SaveChangesAsync();

            if(data.Entity == null || save <= 0)
                  return false;

            return true;
      }

      public async Task<bool> UploadDeviceAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<DeviceDto> GetDeviceByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .Select(e => new DeviceDto(
                  e.guid,
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
            )).FirstOrDefaultAsync() ?? new DeviceDto();
      }

      public async Task<IEnumerable<DeviceDto>> GetDeviceByLocationIdAsync(int LocationId, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.location_id == LocationId)
            .Select(e => new DeviceDto(
                  e.guid,
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
            )).ToArrayAsync();
      }

      public async Task<bool> IsAnyModuleNotSyncAsync(string Mac, int LocationId, DateTime SyncAt, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking().AnyAsync(x => x.location_id == LocationId && x.mac.Equals(Mac) && x.updated_at > SyncAt);
      }

      public async Task SetDeviceSyncStatusAsync(string Mac, string Status, CancellationToken ct = default)
      {
            var entity = await context.Devices
            .Where(x => x.mac.Equals(Mac))
            .FirstOrDefaultAsync();

            if(entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.UpdateSyncStatus(Status);

            var data = context.Devices.Update(entity);
            var save = await context.SaveChangesAsync();

            if(data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);

      }

      public async Task UpdateSyncTimeAsync(string Mac, CancellationToken ct = default)
      {
            var entity = await context.Devices
            .Where(x => x.mac.Equals(Mac))
            .FirstOrDefaultAsync();

            if(entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.UpdateSyncTime();

            var data = context.Devices.Update(entity);
            var save = await context.SaveChangesAsync();

            if(data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.UpdateRecordUnsuccessful);
      }

      public async Task<(string Name,int LocationId)> GetNameAndLocationIdByMacAsync(string Mac, CancellationToken ct = default)
      {
            var res = await context.Devices.AsNoTracking().Where(x => x.mac.Equals(Mac)).Select(x => new {x.name,x.location_id}).FirstOrDefaultAsync();

            if(res is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            return (res.name,res.location_id);
      }

      public async Task<(string Mac, string Type,short ComponentId)> GetMacAndTypeAndComponentIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await context.Devices.AsNoTracking()
            .Where(x => x.guid == guid)
            .Select(x => new { x.mac,x.type,x.component_id })
            .FirstOrDefaultAsync(ct);

            if(res is null)
                  throw new Exception(MessageHelper.Common.NotFound(nameof(guid),guid.ToString()));

            return (res.mac,res.type,res.component_id);
      }

      public async Task<bool> IsAnyByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking().AnyAsync(x => x.guid == guid);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Devices
            .OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .FirstOrDefaultAsync();

            if(entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            context.Devices.Remove(entity);

            await context.SaveChangesAsync(ct);
      }

      public async Task<DeviceDto> GetDeviceByDeviceIdAsync(string DeviceId, CancellationToken ct = default)
      {
            FormattableString sql = context.Database.IsSqlServer()
                  ? (FormattableString)$"""
                        SELECT TOP (1) *
                        FROM device.Devices
                        WHERE type = {DeviceType.amico.ToString()}
                        AND JSON_VALUE(metadata, '$.deviceId') = {DeviceId}
                        ORDER BY id DESC
                        """
                  : (FormattableString)$"""
                        SELECT *
                        FROM device."Devices"
                        WHERE type = {DeviceType.amico.ToString()}
                         AND metadata::jsonb->>'deviceId' = {DeviceId}
                        ORDER BY id DESC
                        LIMIT 1
                        """;

                  var d = await context.Devices
                  .FromSqlInterpolated(sql)
                  .AsNoTracking()
                  .FirstOrDefaultAsync(ct);

                  if(d is null)
                        throw new Exception(MessageHelper.DB.RecordNotFound);

            return new DeviceDto(
                  d.guid,
                  d.name,
                  d.component_id,
                  d.serial_number,
                  d.mac,
                  d.ip,
                  d.port,
                  d.fw,
                  d.type,
                  d.status,
                  d.synced_at,
                  d.location_id,
                  d.metadata,
                  d.is_active,
                  d.is_default
            );
      }
}
