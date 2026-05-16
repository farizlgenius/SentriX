using System;
using System.Reflection.PortableExecutable;
using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Domain.Entities;
using Device.Infrastructure.Persistences;
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

            if(data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new DeviceDto(
                  data.Entity.id,
                  data.Entity.name,data.Entity.serial_number,data.Entity.mac,data.Entity.ip,data.Entity.port,data.Entity.fw,data.Entity.type,data.Entity.status,data.Entity.synced_at,data.Entity.location_id,data.Entity.metadata);
      }


      public async Task<ModuleDto> CreateModuleAsync(Module dto, CancellationToken ct = default)
      {
            var data = await context.Modules.AddAsync(
                  new Persistences.Entities.Module(dto)
            );
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity is null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new ModuleDto(
                  data.Entity.id,
                  data.Entity.name,
                  data.Entity.fw,
                  data.Entity.serial_number,
                  data.Entity.port,
                  data.Entity.address,
                  data.Entity.mac,
                  data.Entity.model,
                  data.Entity.device_id
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
                  x.serial_number,
                  x.mac,
                  x.ip,
                  x.port,
                  x.fw,
                  x.type,
                  x.status,
                  x.synced_at,
                  x.location_id,
                  x.metadata
                  )).FirstOrDefaultAsync() ?? new DeviceDto(
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        0,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        DateTime.UtcNow,
                        0,
                        string.Empty
                  );
      }




      public async Task<int> GetIdByMacAsync(string Mac,CancellationToken ct = default)
      {
            return await context.Devices.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.Equals(Mac))
            .Select(x => x.id)
            .FirstOrDefaultAsync(ct);
      }

      public async Task<string> GetMacByIdAsync(int id,CancellationToken ct =default)
      {
            return await context.Devices.AsNoTracking()
            .Where(x => x.id == id)
            .OrderByDescending(x => x.id)
            .Select(x => x.mac)
            .FirstOrDefaultAsync(ct) ?? string.Empty;
      }

      public async Task<List<ModuleDto>> GetModuleByDeviceIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking()
                  .Where(m => m.device_id == id)
                  .Select(m => new ModuleDto(
                        m.id,
                        m.name,
                        m.fw,
                        m.serial_number,
                        m.port,
                        m.address,
                        m.mac,
                        m.model,
                        m.device_id
                  )).ToListAsync(ct);
      }

      public async Task<ModuleDto> GetModuleByIdAsync(int id,CancellationToken ct= default)
      {
           return await context.Modules.AsNoTracking()
           .OrderByDescending(x => x.id)
           .Where(x => x.id == id)
           .Select(x => new ModuleDto(
            x.id,
            x.name,
            x.fw,
            x.serial_number,
            x.port,
            x.address,
            x.mac,
                        x.model,
            x.device_id
           ))
           .FirstOrDefaultAsync() ?? new ModuleDto(0,string.Empty,string.Empty,
           string.Empty,
           0,
           0,
           string.Empty,
           string.Empty,
           0);
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
                  e.serial_number,
                  e.mac.Replace("_",":"),
                  e.ip,
                  e.port,
                  e.fw,
                  e.type,
                  e.status,
                  e.synced_at,
                  e.location_id,
                  e.metadata
            )).ToListAsync();

            return new Pagination<DeviceDto>(param.pageNumber,param.pageSize,count,(int)Math.Ceiling(count / (double)param.pageSize),res);
      }


      public async Task<bool> IsAnyModuleBySerialNumberAsync(string SerialNumber, CancellationToken ct = default)
      {
            return await context.Modules.AsNoTracking().AnyAsync(x => x.serial_number.Equals(SerialNumber),ct);
      }

      public async Task<bool> IsAnyWithMacAsync(string macAddress, CancellationToken ct)
      {
            return await context.Devices.AsNoTracking().AnyAsync(d => d.mac.Equals(macAddress), ct);
      }

     

      public async Task UpdateIpByMacAsync(string mac, string ip, CancellationToken ct = default)
      {
            var entity = await context.Devices.FirstOrDefaultAsync(d => d.mac == mac, ct);
            if(entity is null)
                  return;

            entity.ip = ip;
            await context.SaveChangesAsync(ct);
      }

      public async Task UpdateModuleAsync(string Mac,int id,string SerialNumber, string Fw, int Port, CancellationToken ct = default)
      {
            var entity = await context.Modules.Where(x => x.id == id && x.devices.mac.Equals(Mac))
            .FirstOrDefaultAsync();

            if(entity == null)
                  return;

            entity.serial_number = SerialNumber;
            entity.fw = Fw;
            entity.port = Port;

            context.Modules.Update(entity);
            await context.SaveChangesAsync(ct);
            
      }

      public async Task UpdatePortByMacAsync(string mac, int port, CancellationToken ct = default)
      {
            var entity = await context.Devices.FirstOrDefaultAsync(d => d.mac == mac, ct);
            if(entity is null)
                  return;

            entity.port = port;
            await context.SaveChangesAsync(ct);
      }

      public async Task VerifyDeviceMemoryAllocateStatusAsync(string mac, string status, CancellationToken ct = default)
      {
            var entity = await context.Devices.FirstOrDefaultAsync(d => d.mac == mac, ct);
            if(entity is null)
                  return;

            entity.UpdateMemoryAllocateStatus(status);
            await context.SaveChangesAsync(ct);
      }
}
