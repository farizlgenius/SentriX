using Core.Application.Interfaces;
using Core.Contract.Dtos.Group;
using Core.Contract.DTOs.Device;
using Core.Contract.DTOs.Group;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class GroupRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Group,Domain.Entities.Group,GroupDto>(
      context,
      d => new Persistences.Entities.Group(d),
      (en,dom) =>
      {
            en.name = dom.Name;

            context.GroupComponents.RemoveRange(en.components);
            en.components.Clear();
            en.components = dom.Components.Select(x => new Persistences.Entities.GroupComponent(x)).ToArray();

            en.updated_at = DateTime.UtcNow;
      },
      GroupProjections.ToDto,
      GroupProjections.ToCommand
),IGroupRepository
{
     

     

      public async Task<IEnumerable<GroupDto>> GetByMacAsync(string mac, CancellationToken ct = default)
      {
            return await Context.Groups
                  .AsNoTracking()
                  .Where(x => x.is_default || x.components.Any(x => x.door.device.mac == mac))
                  .Select(x => new GroupDto(
                        x.guid,
                        x.name,
                        x.components.Select(c => new GroupComponentDto(
                              c.door.guid,
                              c.timezone.guid
                        )).ToList(),
                        x.is_active,
                        x.is_default
                  ))
                  .ToArrayAsync(ct);
      }

     

      public async Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await Context.Groups
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .OrderByDescending(x => x.id)
                  .Select(x => x.id)
                  .ToListAsync(cancellationToken: ct);
      }

      public async Task<IEnumerable<(Guid guid,string mac, string ip, Vendor vendor)>> GetDetailsByGroupGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
{
    var res = await Context.Groups
        .AsNoTracking()
        .Where(x => guids.Contains(x.guid))
        .SelectMany(a => a.components.Select(
            b => new
            {
                b.door.device.guid,
                b.door.device.mac,
                b.door.device.ip,
                b.door.device.vendor
            }
        ))
        // 1. Group the database results strictly by MAC address
        .GroupBy(x => x.mac)
        // 2. Select the first row from each group (discarding any with duplicate MACs)
        .Select(g => g.FirstOrDefault()) 
        .ToArrayAsync(ct);

    return res.Select(x => (x.guid,x.mac, x.ip, x.vendor));
}

      public async Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name],
                  ct
            );
      }

      public async Task InsertGroupComponentAsync(GroupComponent group, CancellationToken ct = default)
      {
            await Context.GroupComponents.AddAsync(
                  new Persistences.Entities.GroupComponent(group),
                  ct
            );

            await Context.SaveChangesAsync(ct);
      }

      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
      {
            return await Context.Groups
                  .AsNoTracking()
                  .AnyAsync(x => x.name == name && x.location_id == locationId, ct);
      }

     

      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Groups
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .AnyAsync(x => x.user_groups.Any() && x.components.Any(), ct);
      }

    

      public Task RemoveGroupComponentAsync(GroupComponent group, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<short> GetFreeExeternalIdByMacAsync(string mac, int max, IEnumerable<short>? exception = null, CancellationToken ct = default)
      {
            var existingIds = await Context.Inputs
                  .AsNoTracking()
                  .Where(x => x.device_module.device.mac == mac)
                  .Select(x => x.external_id!.Value)
                  .ToArrayAsync(ct);

            var existIdsHash = existingIds.ToHashSet();

            if (exception != null)
            {
                  existIdsHash.UnionWith(exception);
            }

            for (short id = 1; id < max; id++)
            {
                  if (!existIdsHash.Contains(id))
                        return id;
            }

            throw new ExceedException(typeof(Persistences.Entities.Group).Name);
      }
}