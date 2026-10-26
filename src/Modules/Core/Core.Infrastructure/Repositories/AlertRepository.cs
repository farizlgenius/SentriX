using Core.Application.Interfaces;
using Core.Contract.DTOs.Alert;
using Core.Infrastructure.Projections;
using Core.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AlertRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Alert,Domain.Entities.Alert,AlertDto>(
      context,
      d => new Persistences.Entities.Alert(d),
      (en,dom) =>
      {
            en.advice = dom.Advice;
            en.alert_level_id = dom.AlertLevelId;
            en.core_event_id = dom.CoreEventId;
            en.updated_at = DateTime.UtcNow;
      },
      AlertProjections.ToDto,
      AlertProjections.ToCommand

),IAlertRepository
{


      public async Task AddLevelAsync(Domain.Entities.AlertLevel domain, CancellationToken ct = default)
      {
            await Context.AlertLevels.AddAsync(
                  new AlertLevel(domain)
                  ,ct);

            await Context.SaveChangesAsync(ct);
      }



      public async Task DeleteLevelAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await Context.AlertLevels
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.ALERT_LEVEL.ToString(),guid.ToString());;

            Context.AlertLevels.Remove(en);

            await Context.SaveChangesAsync(ct);
      }


      public async Task<Pagination<AlertEventDto>> GetEventPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var specialLocationGuid = new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6");

            var query = Context.AlertEvents.AsNoTracking().AsQueryable();

            if (param.locationGuid != specialLocationGuid)
            {
                  query = query.Where(x => x.location == null || x.location.guid == param.locationGuid);
            }

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (Context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.entity.ToString(), pattern) ||
                                  EF.Functions.ILike(x.base_event.ToString(), pattern) ||
                                  EF.Functions.ILike(x.alert_level.ToString(), pattern) ||
                                  EF.Functions.ILike(x.level_color.ToString(), pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.entity.ToString().Contains(search) ||
                                  x.base_event.ToString().Contains(search) ||
                                  x.alert_level.ToString().Contains(search) ||
                                   x.level_color.ToString().Contains(search)
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
                  .Select(x => new AlertEventDto(
                        x.guid,
                        x.timestamp,
                        x.name,
                        x.entity,
                        x.base_event,
                        x.alert_action,
                        x.comment,
                        x.alert_level,
                        x.level_color,
                        x.username,
                        x.is_active,
                        x.is_default
                  )).ToListAsync();

            return new Pagination<AlertEventDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }


      public async Task<int> GetLevelIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.AlertLevels
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync();

            if(res == 0)
                  throw new NotFoundException(EntityType.ALERT_LEVEL.ToString(),guid.ToString());

            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetLevelOptionAsync(CancellationToken ct = default)
      {
            return await Context.AlertLevels
                  .AsNoTracking()
                  .Select(x => new OptionDto(
                        x.name,
                        x.guid,
                        x.color_code
                  )).ToArrayAsync();
      }

      public async Task<Pagination<AlertLevelDto>> GetLevelPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var specialLocationGuid = new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6");

            var query = Context.AlertLevels.AsNoTracking().AsQueryable();

            if (param.locationGuid != specialLocationGuid)
            {
                  query = query.Where(x => x.location == null || x.location.guid == param.locationGuid);
            }

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (Context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.color_code.ToString(), pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.color_code.ToString().Contains(search) 
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
                  .Select(x => new AlertLevelDto(
                        x.guid,
                        x.name,
                        x.color_code,
                        x.is_active,
                        x.is_default
                  )).ToListAsync();

            return new Pagination<AlertLevelDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }


      public async Task<Pagination<AlertDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location == null || x.location.guid == param.locationGuid : null,
                  [x => x.name,x => x.advice],
                  ct
            );
      }



      public async Task<bool> IsAnyLevelByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.AlertLevels
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid,ct);
                  
      }

      public async Task<bool> IsAnyLevelRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.AlertLevels
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.alerts.Any());
      }

      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
           throw new NotImplementedException();
      }


      public async Task UpdateLevelAsync(Domain.Entities.AlertLevel domain, CancellationToken ct = default)
      {
            var en = await Context.AlertLevels
                  .Where(x => x.guid == domain.Guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.ALERT_LEVEL.ToString(),domain.Guid.ToString());

            en.name = domain.Name;
            en.color_code = domain.ColorCode;
            en.updated_at = DateTime.UtcNow;

            Context.AlertLevels.Update(en);

            await Context.SaveChangesAsync(ct);
      }
}