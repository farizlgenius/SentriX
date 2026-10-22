using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Contract.DTOs.DeviceModule;
using Core.Contract.DTOs.Role;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class DeviceRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Device,Domain.Entities.Device,DeviceDto>(
      context,
      d => new Persistences.Entities.Device(d),
      (en,dom) =>
      {
            en.name = dom.Name;
            en.serial_number = dom.SerialNumber;
            en.mac = dom.Mac;
            en.ip = dom.Ip;
            en.port = dom.Port;
            en.firmware = dom.Firmware;
            en.metadata = dom.Metadata;
            en.vendor = dom.Vendor;
            en.location_id = dom.LocationId;
            en.updated_at = DateTime.UtcNow;
      },
      DeviceProjections.ToDto,
      DeviceProjections.ToCommand
),IDeviceRepository
{
     

      public async Task<DeviceDto> GetByMacAsync(string mac, CancellationToken ct = default)
      {
            return await Context.Devices
                  .AsNoTracking()
                  .Where(x => x.mac.Equals(mac))
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
                        x.device_modules.Select(m => new DeviceModuleDto(
                              m.guid,
                              m.name,
                              m.serial_number,
                              m.firmware,
                              m.mac,
                              m.port,
                              m.address,
                              m.model,
                              m.reader_slot,
                              m.output_slot,
                              m.input_slot,
                              m.device.guid,
                              m.device.name,
                              m.location.guid,
                              m.location.name,
                              m.is_active,
                              m.is_default
                              )).ToList(),
                        x.location == null ? Guid.Empty : x.location.guid,
                        x.is_active,
                        x.is_default
                  )).FirstOrDefaultAsync(cancellationToken: ct) ?? throw new NotFoundException(EntityType.DEVICE.ToString(),mac);
      }

      public async Task<int> GetDeviceModuleIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.DeviceModules
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync(cancellationToken: ct);

            if (res == 0)
                  throw new NotFoundException(EntityType.DEVICE_MODULE.ToString(), guid.ToString());

            return res;
      }

   

      public async Task<Guid> GetGuidByMacAsync(string mac, CancellationToken ct = default)
      {
            return await Context.Devices
                  .AsNoTracking()
                  .Where(x => x.mac.Equals(mac))
                  .Select(x => x.guid)
                  .FirstOrDefaultAsync(cancellationToken: ct);
      }

     

      public async Task<string> GetMacByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Devices
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => x.mac)
                  .FirstOrDefaultAsync(cancellationToken: ct) ?? throw new NotFoundException(EntityType.DEVICE.ToString(), guid.ToString());
      }

      public async Task<(string, int)> GetNameAndLocationIdByMacAsync(string mac, CancellationToken ct = default)
      {
            var res = await Context.Devices
                  .AsNoTracking()
                  .Where(x => x.mac.Equals(mac))
                  .Select(x => new { x.name, x.location_id })
                  .FirstOrDefaultAsync(cancellationToken: ct);

            return res == null ? (string.Empty,0) : (res.name,res.location_id);
      }


      public async Task<IEnumerable<OptionDto>> GetOptionByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Devices
                  .AsNoTracking()
                  .Where(x => x.location.guid == guid)
                  .OrderByDescending(x => x.id)
                  .Select(x => new OptionDto(
                        x.name,
                        x.guid,
                        x.mac,
                        null,
                        false
                  )).ToArrayAsync(ct);
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByVendorAndLocationAsync(Vendor vendor, Guid guid, CancellationToken ct = default)
      {
            return await Context.Devices
                  .AsNoTracking()
                  .Where(x => x.vendor == vendor && x.location.guid == guid)
                  .OrderByDescending(x => x.id)
                  .Select(x => new OptionDto(
                        x.name,
                        x.guid,
                        x.mac,
                        null,
                        false
                  )).ToArrayAsync(ct);
      }

       public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name,x => x.mac ,x => x.ip]
                  ,ct);
      }

    

      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = default, CancellationToken ct = default)
      {
            return await Context.Devices
                  .AsNoTracking()
                  .AnyAsync(x => x.name.Equals(name) && x.location_id == locationId, cancellationToken: ct);
      }



      public async Task<bool> IsAnyMacAsync(string mac, CancellationToken ct = default)
      {
            return await Context.Devices
                  .AsNoTracking()
                  .AnyAsync(x => x.mac.Equals(mac), cancellationToken: ct);
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }




      public async Task UpdateConfigurationStatusByMacAsync(string mac, bool isSync,bool isUploaded, CancellationToken ct = default)
      {
            var entity = await Context.Devices
                  .Where(x => x.mac == mac)
                  .OrderByDescending(x => x.id)
                  .FirstOrDefaultAsync(cancellationToken: ct);

            if(entity == null)
                  throw new NotFoundException(EntityType.DEVICE.ToString(),mac);

            entity.configuration_status = isSync ? isUploaded ? DeviceConfigurationStatus.sync : DeviceConfigurationStatus.pending : DeviceConfigurationStatus.reset;
            entity.updated_at = DateTime.UtcNow;
            if(isUploaded)
                  entity.synced_at = DateTime.UtcNow;

            Context.Devices.Update(entity);
            await Context.SaveChangesAsync(ct);
      }
}