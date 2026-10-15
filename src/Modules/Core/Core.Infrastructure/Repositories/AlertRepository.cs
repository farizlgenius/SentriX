using Core.Application.Interfaces;
using Core.Contract.DTOs.Alert;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AlertRepository(CoreDbContext context) : IAlertRepository
{
      public async Task AddAsync(Domain.Entities.Alert entity, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
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

      public async Task<string> GetNameByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
}