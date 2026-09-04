using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class DeviceRepository(CoreDbContext context) : IDeviceRepository
{
      public async Task AddAsync(Device entity, CancellationToken ct = default)
      {
            await context.Devices.AddAsync(
                  new Persistences.Entities.Device(entity), ct);
            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Devices
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Device, guid.ToString());

            context.Devices.Remove(entity);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var entities = await context.Devices
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .ToArrayAsync();

            context.Devices.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Devices
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Device, guid.ToString());

            entity.is_active = false;

            context.Devices.Update(entity);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Devices
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Device, guid.ToString());

            entity.is_active = true;

            context.Devices.Update(entity);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<DeviceDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Devices
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new DeviceDto(
                        x.guid,
                        x.name,
                        x.serial_number,
                        x.mac,
                        x.ip,
                        x.port,
                        x.firmware,
                        x.vendor,
                        x.metadata,
                        x.synced_at,
                        x.configuration_status,
                        x.device_module.Select(d => new DeviceModuleDto(
                              d.guid,
                              d.name,
                              d.serial_number,
                              d.mac,
                              d.firmware,
                              d.port,
                              d.address,
                              d.model
                        )).ToList(),
                        x.location.guid,
                        x.is_active,
                        x.is_default
                  )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Device, guid.ToString());
      }


      public async Task<IEnumerable<DeviceDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
      {
            return await context.Devices
                  .AsNoTracking()
                  .Where(x => x.location_id == locationId)
                  .Select(x => new DeviceDto(
                        x.guid,
                        x.name,
                        x.serial_number,
                        x.mac,
                        x.ip,
                        x.port,
                        x.firmware,
                        x.vendor,
                        x.metadata,
                        x.synced_at,
                        x.configuration_status,
                        x.device_module.Select(d => new DeviceModuleDto(
                              d.guid,
                              d.name,
                              d.serial_number,
                              d.mac,
                              d.firmware,
                              d.port,
                              d.address,
                              d.model
                        )).ToList(),
                        x.location.guid,
                        x.is_active,
                        x.is_default
                  )).ToArrayAsync();
      }

      public async Task<Guid> GetGuidByMacAsync(string mac, CancellationToken ct = default)
      {
            return await context.Devices
                  .AsNoTracking()
                  .Where(x => x.mac.Equals(mac))
                  .Select(x => x.guid)
                  .FirstOrDefaultAsync();
      }

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Devices
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .OrderByDescending(x => x.id)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync();
      }

      public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Devices
                  .Where(x => x.location_id == param.locationGuid)
                  .AsNoTracking()
                  .AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.nam, pattern) ||
                                  EF.Functions.ILike(x.description, pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.description.Contains(search)
                              );
                        }

                  }
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

            var res = await query
                  .AsNoTracking()
                  .OrderByDescending(e => e.created_at)
                  .Skip((param.pageNumber - 1) * param.pageSize)
                  .Take(param.pageSize)
                  .Select(e => new DepartmentDto(
                        e.guid,
                        e.name,
                        e.description,
                        e.company.guid,
                        e.company.name,
                        e.is_active,
                        e.is_default
                  )).ToListAsync();

            return new Pagination<DepartmentDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = default, CancellationToken ct = default)
      {
            return await context.Devices
                  .AsNoTracking()
                  .AnyAsync(x => x.name.Equals(name) && x.location_id == locationId);
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Devices
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid);
      }

      public async Task<bool> IsAnyMacAsync(string mac, CancellationToken ct = default)
      {
            return await context.Devices
                  .AsNoTracking()
                  .AnyAsync(x => x.mac.Equals(mac));
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Devices
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.is_default);
      }

      public async Task UpdateAsync(Device entity, CancellationToken ct = default)
      {
            var en = await context.Devices
                  .Where(x => x.guid == entity.Guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Device, entity.Guid.ToString());

            en.name = entity.Name;
            en.serial_number = entity.SerialNumber;
            en.mac = entity.Mac;
            en.ip = entity.Ip;
            en.port = entity.Port;
            en.firmware = entity.Firmware;
            en.metadata = entity.Metadata;
            en.vendor = entity.Vendor;
            en.location_id = entity.LocationId;

            context.Devices.Update(en);

            await context.SaveChangesAsync(ct);
      }
}