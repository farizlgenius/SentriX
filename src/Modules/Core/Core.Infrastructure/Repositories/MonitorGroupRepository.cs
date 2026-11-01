using Core.Application.Interfaces;
using Core.Contract.DTOs.MonitorGroup;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
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