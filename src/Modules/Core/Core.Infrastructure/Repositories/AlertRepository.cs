using Core.Application.Interfaces;
using Core.Contract.DTOs.Alert;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AlertRepository(CoreDbContext context) : IAlertRepository
{
      public async Task AddAsync(Domain.Entities.Alert entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task AddLevelAsync(Domain.Entities.AlertLevel domain, CancellationToken ct = default)
      {
            await context.AlertLevels.AddAsync(
                  new AlertLevel(domain)
                  ,ct);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Alerts
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Alert.ToString(),guid.ToString());;

            context.Alerts.Remove(en);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteLevelAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.AlertLevels
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.AlertLevel.ToString(),guid.ToString());;

            context.AlertLevels.Remove(en);

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<AlertDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<int>> GetBaseEventIdsByGuids(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await context.BaseEvents
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .Select(x => x.id)
                  .ToArrayAsync();
      }

      public async Task<IEnumerable<AlertDto>> GetByLocationAsync(Guid locationGuid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Pagination<AlertEventDto>> GetEventPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.AlertEvents
                  .Where(x => x.location == null || x.location.guid == param.locationGuid)
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
                                  EF.Functions.ILike(x.entity.ToString(), pattern) ||
                                  EF.Functions.ILike(x.base_event.ToString(), pattern) ||
                                  EF.Functions.ILike(x.comment.ToString(), pattern) ||
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
                                  x.comment.ToString().Contains(search) ||
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
                        x.acknowledge,
                        x.postpone,
                        x.comment,
                        x.alert_level,
                        x.level_color,
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

      public async Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<int> GetLevelIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await context.AlertLevels
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync();

            if(res == 0)
                  throw new NotFoundException(EntityType.AlertLevel.ToString(),guid.ToString());

            return res;
      }

      public async Task<Pagination<AlertLevelDto>> GetLevelPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.AlertLevels
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

      public async Task<string> GetNameByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.AlertLevels
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => x.name)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.AlertLevel.ToString(),guid.ToString());
      }

      public async Task<Pagination<AlertDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Alerts
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
                                  EF.Functions.ILike(x.advice.ToString(), pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.advice.ToString().Contains(search) 
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
                  .Select(x => new AlertDto(
                        x.guid,
                        x.name,
                        x.advice,
                        x.location.guid,
                        x.location.name,
                        x.alert_level.guid,
                        x.alert_level.name,
                        x.alert_base_events.Select(a => new BaseEventDto(
                              a.event_detail.guid,
                              a.event_detail.entity,
                              a.event_detail.detail
                        )).ToList()
                  )).ToListAsync();

            return new Pagination<AlertDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Alerts
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid,ct);
      }

      public async Task<bool> IsAnyLevelByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.AlertLevels
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid,ct);
                  
      }

      public async Task<bool> IsAnyLevelRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.AlertLevels
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.alerts.Any());
      }

      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
             throw new NotImplementedException();
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task UpdateAsync(Domain.Entities.Alert entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task UpdateLevelAsync(Domain.Entities.AlertLevel domain, CancellationToken ct = default)
      {
            var en = await context.AlertLevels
                  .Where(x => x.guid == domain.Guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.AlertLevel.ToString(),domain.Guid.ToString());

            en.name = domain.Name;
            en.color_code = domain.ColorCode;
            en.updated_at = DateTime.UtcNow;

            context.AlertLevels.Update(en);

            await context.SaveChangesAsync(ct);
      }
}