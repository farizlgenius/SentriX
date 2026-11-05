using Core.Application.Interfaces;
using Core.Contract.DTOs.MonitorGroup;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class MonitorGroupRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.MonitorGroup, Domain.Entities.MonitorGroup, MonitorGroupDto>(
      context,
      d => new Persistences.Entities.MonitorGroup(d),
      (en, dom) =>
      {
            en.name = dom.Name;
            en.device_id = dom.DeviceId;
            en.updated_at = DateTime.UtcNow;
      },
      MonitorGroupProjections.ToDto,
      MonitorGroupProjections.ToCommand
), IMonitorGroupRepository
{
      public async Task<IEnumerable<MonitorGroupDto>> GetByDeviceAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.MonitorGroups
                  .AsNoTracking()
                  .Where(x => x.device.guid == guid)
                  .Select(MonitorGroupProjections.ToDto)
                  .ToArrayAsync(ct);
      }

      public async Task<IEnumerable<(Guid Guid,short ExternalId,MonitorType Type)>> GetComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorAsync(short externalId, short deviceExternalId, Vendor vendor, CancellationToken ct = default)
      {
            var res = await Context.MonitorGroups
                  .AsNoTracking()
                  .Where(x => x.external_id == externalId && x.device.external_id == deviceExternalId && x.device.vendor == vendor)
                  .SelectMany(x => x.components.Select(c => new 
                  {
                        Guid = c.door == null ? c.input == null ? Guid.Empty : c.input.guid : c.door.guid,
                        ExternalId = c.door == null ? c.input == null ? -1 : c.input.external_id : c.door.external_id ,
                        Type=c.type,
                        })).ToArrayAsync(ct);

            return [.. res.Select(x => (x.Guid,x.ExternalId ?? -1,x.Type))];
      }

      public async Task<short> GetFreeExternalIdByMacAsync(string mac, int max, IEnumerable<short> excepions, CancellationToken ct = default)
      {
            var existingIds = await Context.MonitorGroups
                  .AsNoTracking()
                  .Where(x => x.device.mac == mac)
                  .Select(x => x.external_id!.Value)
                  .ToArrayAsync(ct);

            var existIdsHash = existingIds.ToHashSet();

            if (excepions != null)
            {
                  existIdsHash.UnionWith(excepions);
            }

            for (short id = 1; id < max; id++)
            {
                  if (!existIdsHash.Contains(id))
                        return id;
            }

            throw new ExceedException(typeof(MonitorGroup).Name);
      }

      public async Task<Guid> GetGuidByExternalIdAndMacAndVendorAsync(short externalId, string mac, Vendor vendor, CancellationToken ct = default)
      {
           return await Context.MonitorGroups
            .AsNoTracking()
            .Where(x => x.external_id == externalId && x.device.mac == mac && x.device.vendor == vendor)
            .Select(x => x.guid)
            .FirstOrDefaultAsync(ct);
      }

      public async Task<string> GetNameByExternalIdAndMacAndVendorAsync(short ExternalId, string Mac, Vendor Vendor, CancellationToken ct = default)
      {
            return await Context.MonitorGroups
                  .AsNoTracking()
                  .Where(x => x.external_id == ExternalId && x.device.mac == Mac && x.device.vendor == Vendor)
                  .Select(x => x.name)
                  .FirstOrDefaultAsync(ct) ?? "Unknown";
      }

      public async Task<Pagination<MonitorGroupDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name],
                  ct
                  );
      }

      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}