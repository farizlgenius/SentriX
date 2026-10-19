using Core.Application.Interfaces;
using Core.Contract.DTOs.Action;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class ActionRepository(CoreDbContext context) : IActionRepository
{
      public async Task AddAsync(Actions entity, CancellationToken ct = default)
      {
            await context.Action.AddAsync(
                  new Persistences.Entities.Action(entity)
                  ,ct);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Action
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync();

            if(en == null)
                  throw new NotFoundException(EntityType.ACTION.ToString(),guid.ToString());

            context.Action.Remove(en);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var entities = await context.Action
                  .Where(x => guids.Contains(x.guid) && x.is_default == false)
                  .ToArrayAsync(ct);

            context.Action.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Action
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.ACTION.ToString(), guid.ToString());

            en.is_active = false;

            context.Action.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
             var en = await context.Action
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.ACTION.ToString(), guid.ToString());

            en.is_active = true;

            context.Action.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<ActionDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Action
                  .AsNoTracking()
                  .Select(x => new ActionDto(
                        x.guid,
                        x.name,
                        x.device.guid,
                        x.type,
                        x.metadata,
                        x.is_active,
                        x.is_default
                  )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.ACTION.ToString(),guid.ToString());
      }

      public async Task<IEnumerable<ActionDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default)
      {
            return await context.Action
                  .AsNoTracking()
                  .Where(x => x.location.guid == locationGuid)
                  .Select(x => new ActionDto(
                        x.guid,
                        x.name,
                        x.device.guid,
                        x.type,
                        x.metadata,
                        x.is_active,
                        x.is_default
                  )).ToArrayAsync();
      }

      public async Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            var res = await context.Action
                  .AsNoTracking()
                  .Where(x => x.id == id)
                  .Select(x => x.guid)
                  .FirstOrDefaultAsync();

            if(res == Guid.Empty)
                  throw new NotFoundException(EntityType.ACTION.ToString(),id.ToString());

            return res;
      }

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await context.Action
            .AsNoTracking()
            .Where(x => x.guid == guid)
            .Select(x => x.id)
            .FirstOrDefaultAsync();

            if(res == 0)
                  throw new NotFoundException(EntityType.ACTION.ToString(),guid.ToString());

            return res;
      }

      public async Task<string> GetNameByGuidAsync(Guid guid, CancellationToken ct = default)
      {
           return await context.Action
            .AsNoTracking()
            .Where(x => x.guid == guid)
            .Select(x => x.name)
            .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.ACTION.ToString(),guid.ToString());
      }

      public async Task<Pagination<ActionDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
           var query = context.Action
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
                                  EF.Functions.ILike(x.type.ToString(), pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.type.ToString().Contains(search) 
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
                  .Select(x => new ActionDto(
                        x.guid,
                        x.name,
                        x.device.guid,
                        x.type,
                        x.metadata,
                        x.is_active,
                        x.is_default
                  )).ToListAsync();

            return new Pagination<ActionDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Action
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid);
      }

      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Action
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.procedure_actions.Any(),ct);
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Action
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.is_default);
      }

      public Task UpdateAsync(Actions entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}