using Core.Application.Interfaces;
using Core.Contract.Dtos.Group;
using Core.Contract.DTOs.Group;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class GroupRepository(CoreDbContext context) : IGroupRepository
{
      public async Task AddAsync(Group entity, CancellationToken ct = default)
      {
            await context.Groups.AddAsync(
                  new Persistences.Entities.Group(entity), ct
            );

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Groups
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Group, guid.ToString());

            context.Groups.Remove(entity);
            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var entities = await context.Groups
                  .Where(x => guids.Contains(x.guid))
                  .ToListAsync(ct);

            context.Groups.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Groups
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Group, guid.ToString());

            entity.is_active = false;
            await context.SaveChangesAsync(ct);
            return true;
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Groups
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Group, guid.ToString());

            entity.is_active = true;
            await context.SaveChangesAsync(ct);
            return true;
      }

      public async Task<GroupDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Groups
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
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
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Group, guid.ToString());
      }

      public async Task<IEnumerable<GroupDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
      {
            return await context.Groups
                  .AsNoTracking()
                  .Where(x => x.location_id == locationId)
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

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await context.Groups
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync(ct);

            if (res == 0)
                  throw new NotFoundException(EntityType.Group, guid.ToString());

            return res;
      }

      public async Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await context.Groups
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .OrderByDescending(x => x.id)
                  .Select(x => x.id)
                  .ToListAsync();
      }

      public async Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Groups
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

            var res = await query
                  .AsNoTracking()
                  .OrderByDescending(e => e.created_at)
                  .Skip((param.pageNumber - 1) * param.pageSize)
                  .Take(param.pageSize)
                  .Select(x => new GroupDto(
                        x.guid,
                        x.name,
                        x.components.Select(c => new GroupComponentDto(
                              c.door.guid,
                              c.timezone.guid
                        )).ToList(),
                        x.is_active,
                        x.is_default
                  )).ToListAsync();

            return new Pagination<GroupDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
      {
            return await context.Groups
                  .AsNoTracking()
                  .AnyAsync(x => x.name == name && x.location_id == locationId, ct);
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Groups
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .AnyAsync(ct);
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Groups
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .AnyAsync(x => x.is_default, ct);
      }

      public async Task UpdateAsync(Group entity, CancellationToken ct = default)
      {
            var en = await context.Groups
                  .Where(x => x.guid == entity.Guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Group, entity.Guid.ToString());

            en.name = entity.Name;

            context.GroupComponents.RemoveRange(en.components);
            en.components.Clear();
            en.components = entity.Components.Select(x => new Persistences.Entities.GroupComponent(x)).ToArray();

            en.updated_at = DateTime.UtcNow;

            context.Groups.Update(en);
            await context.SaveChangesAsync(ct);
      }
}