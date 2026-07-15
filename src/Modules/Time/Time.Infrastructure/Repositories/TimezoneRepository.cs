using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using Time.Application.Interfaces;
using Time.Contract.DTOs;
using Time.Infrastructure.Persistences;
using Time.Infrastructure.Persistences.Entities;

namespace Time.Infrastructure.Repositories;

public sealed class TimezoneRepository(TimeDbContext context) : ITimeZoneRepository
{
      public async Task AddAsync(Domain.Entities.TimeZone timezone, CancellationToken ct = default)
      {
            await context.Timezones.AddAsync(
                  new Persistences.Entities.TimeZone(timezone),
                  ct
            );

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Timezones.OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .FirstOrDefaultAsync();

            if (entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            context.Timezones.Remove(entity);
            await context.SaveChangesAsync(ct);

      }

      public async Task<TimeZoneDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Timezones.AsNoTracking()
            .Include(x => x.intervals).ThenInclude(x => x.days)
            .OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .Select(x => new TimeZoneDto(
                  x.guid,
                  x.component_id,
                  x.name,
                  x.mode,
                  x.active,
                  x.deactive,
                  x.intervals.Select(x => new IntervalDto(
                        x.guid,
                         x.component_id,
                        new DaysInWeekDto(
                              x.days.guid,
                              x.days.sunday,
                              x.days.monday,
                              x.days.tuesday,
                              x.days.wednesday,
                              x.days.thursday,
                              x.days.friday,
                              x.days.saturday
                              ),
                        x.days_detail,
                        x.start,
                        x.end
                  )).ToList(),
                  x.location_id,
                  x.is_active,
                  x.is_default
                  )).FirstOrDefaultAsync() ?? new TimeZoneDto();
      }

      public async Task<short> GetLowestTimeZoneComponentIdAsync(int location_id,CancellationToken ct = default)
      {
            return (short)await ComponentHelper.LowestUnassignedNumberStartOneWithFileterAsync<Persistences.Entities.TimeZone>(
                  context,
                  x => x.location_id == location_id || x.location_id == 0,
                  x => x.component_id,
                  255,
                  ct
                  );
      }

      public async Task<short> GetLowestIntervalComponentIdAsync(CancellationToken ct = default)
      {
            return (short)await ComponentHelper.LowestUnassignedNumberStartOneAsync<Persistences.Entities.Interval>(
                  context,
                  x => x.component_id,
                  255,
                  ct
                  );
      }

      public async Task<short> GetLowestIntervalComponentIdExceptStartFromOneAsync(
            List<short> Excepts,
            Guid TzGuid,
            CancellationToken ct = default
            )
      {
            return (short)await ComponentHelper.LowestUnassignedNumberExceptStartFromOneAsync<Persistences.Entities.Interval>(
                  context,
                  Excepts,
                  x => x.timezone_guid == TzGuid,
                  x => x.component_id,
                  255,
                  ct
            );
      }



      public async Task<Pagination<TimeZoneDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Timezones.AsNoTracking().Where(x => x.location_id == param.locationId || x.location_id == 0).AsQueryable();

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
                                  EF.Functions.ILike(x.active.ToString(), pattern) ||
                                  EF.Functions.ILike(x.deactive.ToString(), pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.active.ToString().Contains(search) ||
                                  x.deactive.ToString().Contains(search)
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

            var totalItems = await query.CountAsync();
            var items = await query.OrderByDescending(r => r.id)
            .Skip((param.pageNumber - 1) * param.pageSize)
            .Take(param.pageSize)
             .Select(x => new TimeZoneDto(
                  x.guid,
                  x.component_id,
                  x.name,
                  x.mode,
                  x.active,
                  x.deactive,
                  x.intervals.Select(x => new IntervalDto(
                        x.guid,
                         x.component_id,
                        new DaysInWeekDto(
                              x.days.guid,
                              x.days.sunday,
                              x.days.monday,
                              x.days.tuesday,
                              x.days.wednesday,
                              x.days.thursday,
                              x.days.friday,
                              x.days.saturday
                              ),
                        x.days_detail,
                        x.start,
                        x.end
                  )).ToList(),
                  x.location_id,
                  x.is_active,
                  x.is_default
                  ))
            .ToListAsync(ct);

            return new Pagination<TimeZoneDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<IEnumerable<TimeZoneDto>> GetTimeZoneByLocationIdAsync(int locationId, CancellationToken ct = default)
      {
            return await context.Timezones.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.location_id == locationId || x.location_id == 0)
           .Select(x => new TimeZoneDto(
                  x.guid,
                  x.component_id,
                  x.name,
                  x.mode,
                  x.active,
                  x.deactive,
                  x.intervals.Select(x => new IntervalDto(
                        x.guid,
                        x.component_id,
                        new DaysInWeekDto(
                              x.days.guid,
                              x.days.sunday,
                              x.days.monday,
                              x.days.tuesday,
                              x.days.wednesday,
                              x.days.thursday,
                              x.days.friday,
                              x.days.saturday
                              ),
                        x.days_detail,
                        x.start,
                        x.end
                  )).ToList(),
                  x.location_id,
                  x.is_active,
                  x.is_default
                  )).ToArrayAsync(ct);
      }

      public async Task<IEnumerable<OptionDto>> GetTimezoneOptionByLocationIdAsync(int locationId, CancellationToken ct = default)
      {
            return await context.Timezones.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.location_id == locationId || x.location_id == 0)
            .Select(x => new OptionDto(
                  x.name,
                  x.component_id,
                  string.Empty,
                  x.guid
                  )).ToArrayAsync(ct);
      }

      public async Task<bool> IsAnyTimeZoneNotSyncAsync(int LocationId, DateTime SyncAt, CancellationToken ct = default)
      {
            return await context.Timezones.AsNoTracking()
            .AnyAsync(x => (x.location_id == LocationId && x.location_id == 0) || x.updated_at > SyncAt);
      }

      public async Task<bool> IsAnyNameAsync(string name, CancellationToken ct = default)
      {
            return await context.Timezones.AsNoTracking().AnyAsync(x => x.name.Equals(name));
      }

      public async Task UpdateAsync(Domain.Entities.TimeZone timezone, CancellationToken ct = default)
      {
            var entity = await context.Timezones
            .Where(x => x.guid == timezone.Guid)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync();

            if(entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(timezone);

            context.Timezones.Update(entity);

            await context.SaveChangesAsync(ct);
      }

      public async Task<int> CountTimeZoneByLocationIdAsync(int location_id, CancellationToken ct = default)
      {
            return await context.Timezones.AsNoTracking().Where(x => x.location_id == location_id || x.location_id == 0).CountAsync();
      }




}