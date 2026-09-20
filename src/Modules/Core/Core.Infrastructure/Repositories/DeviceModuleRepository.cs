using Core.Application.Interfaces;
using Core.Contract.DTOs.DeviceModule;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class DeviceModuleRepository(CoreDbContext context) : IDeviceModuleRepository
{
      public async Task AddAsync(DeviceModule entity, CancellationToken ct = default)
      {
            await context.DeviceModules.AddAsync(
                  new Persistences.Entities.DeviceModule(entity)
                  , ct);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.DeviceModules
                 .OrderByDescending(x => x.id)
                 .Where(x => x.guid == guid)
                 .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.DeviceModule, guid.ToString());

            context.DeviceModules.Remove(entity);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var entities = await context.DeviceModules
                  .Where(x => guids.Contains(x.guid))
                  .ToArrayAsync();

            context.DeviceModules.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.DeviceModules
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.DeviceModule, guid.ToString());

            en.is_active = false;

            context.DeviceModules.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.DeviceModules
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.DeviceModule, guid.ToString());

            en.is_active = true;

            context.DeviceModules.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<DeviceModuleDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.DeviceModules
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new DeviceModuleDto(
                        x.guid,
                        x.name,
                        x.serial_number,
                        x.firmware,
                        x.mac,
                        x.port,
                        x.address,
                        x.model,
                        x.reader_slot,
                        x.output_slot,
                        x.input_slot,
                        x.device.guid,
                        x.device.name,
                        x.location.guid,
                        x.location.name,
                        x.is_active,
                        x.is_default
                  )).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.DeviceModule, guid.ToString());
      }

      public async Task<IEnumerable<DeviceModuleDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
      {
            return await context.DeviceModules
                  .AsNoTracking()
                  .Where(x => x.location_id == locationId)
                  .Select(x => new DeviceModuleDto(
                        x.guid,
                        x.name,
                        x.serial_number,
                        x.firmware,
                        x.mac,
                        x.port,
                        x.address,
                        x.model,
                        x.reader_slot,
                        x.output_slot,
                        x.input_slot,
                        x.device.guid,
                        x.device.name,
                        x.location.guid,
                        x.location.name,
                        x.is_active,
                        x.is_default
                  )).ToArrayAsync(ct);
      }

      public async Task<IEnumerable<DeviceModuleDto>> GetByVendorAndLocationAsync(int locationId, SharedKernel.Enums.Vendor vedor, CancellationToken ct = default)
      {
            return await context.DeviceModules
                  .AsNoTracking()
                  .Where(x => x.device.vendor == vedor && x.location_id == locationId)
                  .Select(x => new DeviceModuleDto(
                        x.guid,
                        x.name,
                        x.serial_number,
                        x.firmware,
                        x.mac,
                        x.port,
                        x.address,
                        x.model,
                        x.reader_slot,
                        x.output_slot,
                        x.input_slot,
                        x.device.guid,
                        x.device.name,
                        x.location.guid,
                        x.location.name,
                        x.is_active,
                        x.is_default
                  )).ToArrayAsync();
      }

      public async Task<int> GetDeviceModuleIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await context.DeviceModules
             .AsNoTracking()
             .Where(x => x.guid == guid)
             .Select(x => x.id)
             .FirstOrDefaultAsync();

            if (res == 0)
                  throw new NotFoundException(EntityType.DeviceModule, guid.ToString());

            return res;
      }

      public async Task<Dictionary<Guid, int>> GetDeviceModuleIdsMapGuidsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await context.DeviceModules
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .ToDictionaryAsync(x => x.guid, x => x.id, ct);
      }

      public Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await context.DeviceModules
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync();

            if (res == 0)
                  throw new NotFoundException(EntityType.DeviceModule, guid.ToString());

            return res;
      }

      public async Task<Pagination<DeviceModuleDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.DeviceModules
                  .Where(x => x.location.guid == param.locationGuid)
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
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.serial_number, pattern) ||
                                  EF.Functions.ILike(x.firmware, pattern) ||
                                  EF.Functions.ILike(x.mac, pattern) ||
                                  EF.Functions.ILike(x.port.ToString(), pattern) ||
                                  EF.Functions.ILike(x.address.ToString(), pattern) ||
                                  EF.Functions.ILike(x.model.ToString(), pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.serial_number.Contains(search) ||
                                  x.firmware.Contains(search) ||
                                  x.mac.Contains(search) ||
                                  x.port.ToString().Contains(search) ||
                                  x.address.ToString().Contains(search) ||
                                  x.model.ToString().Contains(search)
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
                 .Select(x => new DeviceModuleDto(
                              x.guid,
                              x.name,
                              x.serial_number,
                              x.firmware,
                              x.mac,
                              x.port,
                              x.address,
                              x.model,
                              x.reader_slot,
                              x.output_slot,
                              x.input_slot,
                              x.device.guid,
                              x.device.name,
                              x.location.guid,
                              x.location.name,
                              x.is_active,
                              x.is_default
                            )).ToListAsync();

            return new Pagination<DeviceModuleDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
      {
            return await context.DeviceModules
                  .AsNoTracking()
                  .AnyAsync(x => x.name.Equals(name) && x.location_id == locationId);

      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.DeviceModules
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid);
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.DeviceModules
                  .AsNoTracking()
                  .AnyAsync(x => x.is_default);
      }

      public async Task UpdateAsync(DeviceModule entity, CancellationToken ct = default)
      {
            var en = await context.DeviceModules
                   .AsNoTracking()
                   .Where(x => x.guid == entity.Guid)
                   .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.DeviceModule, entity.Guid.ToString());

            en.name = entity.Name;
            en.serial_number = entity.SerialNumber;
            en.firmware = entity.Firmware;
            en.mac = entity.Mac;
            en.port = entity.Port;
            en.address = entity.Address;
            en.model = entity.Model;
            en.reader_slot = entity.ReaderSlot;
            en.input_slot = entity.InputSlot;
            en.output_slot = entity.OutputSlot;
            en.updated_at = DateTime.UtcNow;


            context.DeviceModules.Update(en);

            await context.SaveChangesAsync(ct);
      }
}