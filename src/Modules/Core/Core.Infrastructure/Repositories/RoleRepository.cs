using Core.Application.Interfaces;
using Core.Contract.DTOs.Role;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class RoleRepository(CoreDbContext context) : IRoleRepository
{
      public async Task AddAsync(Role entity, CancellationToken ct = default)
      {
            await context.Roles.AddAsync(
                  new Persistences.Entities.Role(entity), ct
            );

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Roles
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync();

            context.Roles.Remove(entity ?? throw new NotFoundException(EntityType.Role, guid.ToString()));

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var entities = await context.Roles
                  .Where(x => guids.Contains(x.guid) && x.is_default == false)
                  .ToArrayAsync(ct);

            context.Roles.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Roles
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Location, guid.ToString());

            en.is_active = false;

            context.Roles.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Roles
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Location, guid.ToString());

            en.is_active = true;

            context.Roles.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<RoleDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Roles
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new RoleDto(
                    x.guid,
                    x.name,
                    x.permissions.Select(p => new PermissionDto(
                        p.guid,
                        p.feature_guid,
                        p.role_guid,
                        p.is_enabled,
                        p.is_created,
                        p.is_updated,
                        p.is_deleted
                    )).ToList(),
                    x.location_guid,
                    x.is_active,
                    x.is_default
                  )).FirstOrDefaultAsync() ?? throw new NotFoundException(nameof(Location), guid.ToString());
      }

      public async Task<Pagination<RoleDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Roles
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
                  .Select(e => new RoleDto(
                        e.guid,
                        e.name,
                        e.permissions.Select(p => new PermissionDto(
                              p.guid,
                              p.feature_guid,
                              p.role_guid,
                              p.is_enabled,
                              p.is_created,
                              p.is_updated,
                              p.is_deleted
                        )).ToList(),
                        e.location_guid,
                        e.is_active,
                        e.is_default
                  )).ToListAsync();

            return new Pagination<RoleDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public async Task<bool> IsAnyByNameAndLocationGuidAsync(string name, Guid locationGuid, CancellationToken ct = default)
      {
            return await context.Roles
                  .AsNoTracking()
                  .AnyAsync(x => x.name.Equals(name));
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Roles
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid);
      }

      public async Task<bool> IsAnyNameAsync(string Name, Guid locationGuid, CancellationToken ct = default)
      {
            return await context.Roles
                  .AsNoTracking()
                  .AnyAsync(x => x.name.Equals(Name) && x.location_guid == locationGuid);
      }

      public async Task<bool> IsAnyOperatorAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Roles
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.operators.Any());

      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Companies
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.is_default);
      }

      public async Task UpdateAsync(Role entity, CancellationToken ct = default)
      {
            var en = await context.Roles
                  .Include(x => x.permissions)
                  .Where(x => x.guid == entity.Guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Location, entity.Guid.ToString());

            // Delete old permission
            context.Permissions.RemoveRange(en.permissions);

            en.name = entity.Name;
            en.permissions = entity.Permissions.Select(x => new Core.Infrastructure.Persistences.Entities.Permission(
                  x
            )).ToList();

            context.Roles.Update(en);

            await context.SaveChangesAsync(ct);
      }
}