using Door.Contract.DTOs;
using Group.Application.Interfaces;
using Group.Contract.DTOs;
using Group.Domain.Entities;
using Group.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Group.Infrastructure.Repositories;

public sealed class GroupRepository(GroupDbContext context) : IGroupRepository
{
      public async Task CreateAsync(Groups domain, CancellationToken ct = default)
      {
            await context.Groups.AddAsync(
                  new Persistences.Entities.Groups(domain)
            );

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.Groups.OrderByDescending(x => x.id)
            .Where(x => x.id == id)
            .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            context.Groups.Remove(entity);
            await context.SaveChangesAsync(ct);


      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Groups.OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            context.Groups.Remove(entity);
            await context.SaveChangesAsync(ct);
      }

      public async Task<GroupDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking()
            .Include(x => x.group_doors)
            .Where(x => x.guid == guid)
            .OrderByDescending(x => x.id)
            .Select(x => new GroupDto(
                  x.guid,
                  x.name,
                  x.group_doors.Select(g => new GroupDoorDto(
                        g.mac,
                        g.device_component_id,
                        g.door_component_id,
                        g.timezone_component_id,
                        g.type
                  )).ToList(),
                  x.location_id,
                  x.is_active,
                  x.is_default
            ))
            .FirstOrDefaultAsync(ct) ?? new GroupDto();
      }

      public async Task<GroupDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking()
            .Include(x => x.group_doors)
            .Where(x => x.id == id)
            .OrderByDescending(x => x.id)
            .Select(x => new GroupDto(
                  x.guid,
                  x.name,
                  x.group_doors.Select(g => new GroupDoorDto(
                        g.mac,
                        g.device_component_id,
                        g.door_component_id,
                        g.timezone_component_id,
                        g.type
                  )).ToList(),
                  x.location_id,
                  x.is_active,
                  x.is_default
            ))
            .FirstOrDefaultAsync(ct) ?? new GroupDto();
      }

      public async Task<IEnumerable<GroupDto>> GetByLocationIdAsync(int location, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking()
            .Where(x => x.location_id == location || x.location_id == 0)
            .Select(x => new GroupDto(
                  x.guid,
                  x.name,
                  x.group_doors.Select(g => new GroupDoorDto(
                        g.mac,
                        g.device_component_id,
                        g.door_component_id,
                        g.timezone_component_id,
                        g.type
                  )).ToList(),
                  x.location_id,
                  x.is_active
            )).ToArrayAsync();
      }

      public async Task<IEnumerable<GroupSplitByMacDto>> GetByRangeGuidAsync(List<Guid> guids, CancellationToken ct = default)
      {
             return await context.GroupDoors
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.group_guid))
                  .GroupBy(x => new { x.mac, x.type,x.device_component_id })
                  .Select(g => new GroupSplitByMacDto(
                        g.Key.mac,
                        g.Key.type,
                        g.Key.device_component_id,
                        g.Select(x => x.groups.component_id)
                        .ToList()
                  ))
                  .ToArrayAsync(ct);

      }

      public async Task<IEnumerable<GroupDto>> GetGroupByMacAsync(string Mac,string Type,CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking()
            .Where(x => x.group_doors.Any(g => g.mac.Equals(Mac)))
            .Select(x => new GroupDto(
                  x.guid,
                  x.name,
                 x.group_doors.Select(g => new GroupDoorDto(
                        g.mac,
                        g.device_component_id,
                        g.door_component_id,
                        g.timezone_component_id,
                        g.type
                  )).ToList(),
                  x.location_id,
                  x.is_active
            )).ToArrayAsync(ct);
      }

      public async Task<IEnumerable<(Guid guid,short componentId)>> GetGroupGuidAndComponentIdsByMacAsync(string Mac, CancellationToken ct = default)
      {
           var res = await context.Groups.AsNoTracking()
            .Where(x => x.group_doors.Any(s => s.mac.Equals(Mac)))
            .Select(x =>new {x.guid,x.component_id})
            .ToArrayAsync(ct);

            return res.Select(x => (x.guid,x.component_id));

      }


      public async Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Groups.AsNoTracking()
            .Include(x => x.group_doors)
            .Where(x => x.location_id == param.locationId || x.location_id == 0).AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) 
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

            var res = await query.AsNoTracking()
            .OrderByDescending(e => e.created_at)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
            .Select(x => new GroupDto(
                  x.guid,
                  x.name,
                  x.group_doors.Select(g => new GroupDoorDto(
                        g.mac,
                        g.device_component_id,
                        g.door_component_id,
                        g.timezone_component_id,
                        g.type
                  )).ToList(),
                  x.location_id,
                  x.is_active
            )).ToListAsync(ct);

            return new Pagination<GroupDto>(param.pageNumber,param.pageSize,count,(int)Math.Ceiling(count / (double)param.pageSize),res);
      }

      public async Task<bool> IsAnyByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking().AnyAsync(x => x.guid == guid);
      }

      public async Task<bool> IsAnyByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking().AnyAsync(x => x.id == id);
      }

      public async Task<bool> IsAnyGroupNotSyncQueryAsync(int LocationId, DateTime SyncAt, CancellationToken ct = default)
      {
            return await context.Groups.AsNoTracking().AnyAsync(x => (x.location_id == LocationId || x.location_id == 0) && x.updated_at > SyncAt);
      }

      public async Task<IEnumerable<string>> MacsByGroupIdAsync(IEnumerable<int> Ids, CancellationToken ct = default)
      {
            return await context.Groups.Where(x => Ids.Contains(x.id))
                  .SelectMany(x => x.group_doors.Select(x => x.mac)).ToArrayAsync();
      }

      public async Task UpdateAsync(Groups dto, CancellationToken ct = default)
      {
            var entity = await context.Groups
            .Include(x => x.group_doors)
            .OrderByDescending(x => x.id)
            .Where(x => x.guid == dto.Guid)
            .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(dto);

            context.GroupDoors.RemoveRange(entity.group_doors);

            entity.group_doors = dto.GroupDoors.Select(x => new Persistences.Entities.GroupDoor(x)).ToList();

            context.Groups.Update(entity);
           await context.SaveChangesAsync(ct);

      }
}