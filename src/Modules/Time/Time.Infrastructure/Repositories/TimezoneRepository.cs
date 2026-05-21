using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using Time.Application.Interfaces;
using Time.Contract.DTOs;
using Time.Infrastructure.Persistences;
using Time.Infrastructure.Persistences.Entities;

namespace Time.Infrastructure.Repositories;

public sealed class TimezoneRepository(TimeDbContext context) : ITimezoneRepository
{
      public async Task<TimezoneDto> CreateAsync(Domain.Entities.Timezone timezone, CancellationToken ct = default)
      {
            var data = await context.Timezones.AddAsync(
                  new Persistences.Entities.Timezone(timezone)
            );

            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return  await context.Timezones.AsNoTracking()
            .Include(x => x.intervals).ThenInclude(x => x.days)
            .Where(x => x.id == data.Entity.id)
            .Select(x => new TimezoneDto(
                  x.id,
                  x.component_id,
                  x.name,
                  x.mode,
                  x.active,
                  x.deactive,
                  x.intervals.Select(x => new IntervalDto(
                        x.id,
                        x.component_id,
                        new DaysInWeekDto(
                              x.days.id,
                              x.days.component_id,
                              x.days.sunday,
                              x.days.monday,
                              x.days.tuesday,
                              x.days.wednesday,
                              x.days.thursday,
                              x.days.friday,
                              x.days.saturday,
                              x.location_id
                              ),
                        x.days_detail,
                        x.start,
                        x.end,
                        x.location_id,
                        x.is_active
                  )).ToList(),
                  x.location_id,
                  x.is_active
                  )).FirstOrDefaultAsync() ?? new TimezoneDto(
                        0,
                        0,
                        string.Empty,
                        0,
                        string.Empty,
                        string.Empty,
                        new List<IntervalDto>(),
                        0,
                        false
                        );
      }

      public async Task<TimezoneDto> DeleteByIdAsync(int id, CancellationToken ct = default)
      {
            var entity = await context.Timezones.OrderByDescending(x => x.id)
            .Where(x => x.id == id)
            .FirstOrDefaultAsync();

            if(entity == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var data = context.Timezones.Remove(entity);
            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return new TimezoneDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.mode,
                  data.Entity.active,
                  data.Entity.deactive,
                  data.Entity.intervals.Select(x => new IntervalDto(
                        x.id,
                        x.component_id,
                        new DaysInWeekDto(
                              x.days.id,
                              x.days.component_id,
                              x.days.sunday,
                              x.days.monday,
                              x.days.tuesday,
                              x.days.wednesday,
                              x.days.thursday,
                              x.days.friday,
                              x.days.saturday,
                              x.days.location_id
                              ),
                        x.days_detail,
                        x.start,
                        x.end,
                        x.location_id,
                        x.is_active
                        )).ToList(),
                  data.Entity.location_id,
                  data.Entity.is_active
            );

      }

      public async Task<TimezoneDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Timezones.AsNoTracking()
            .Include(x => x.intervals).ThenInclude(x => x.days)
            .OrderByDescending(x => x.id)
            .Where(x => x.id == id)
            .Select(x => new TimezoneDto(
                  x.id,
                  x.component_id,
                  x.name,
                  x.mode,
                  x.active,
                  x.deactive,
                  x.intervals.Select(x => new IntervalDto(
                        x.id,
                        x.component_id,
                        new DaysInWeekDto(
                              x.days.id,
                              x.days.component_id,
                              x.days.sunday,
                              x.days.monday,
                              x.days.tuesday,
                              x.days.wednesday,
                              x.days.thursday,
                              x.days.friday,
                              x.days.saturday,
                              x.location_id
                              ),
                        x.days_detail,
                        x.start,
                        x.end,
                        x.location_id,
                        x.is_active
                  )).ToList(),
                  x.location_id,
                  x.is_active
                  )).FirstOrDefaultAsync() ?? new TimezoneDto(
                        0,
                        0,
                        string.Empty,
                        0,
                        string.Empty,
                        string.Empty,
                        new List<IntervalDto>(),
                        0,
                        false
                        );
      }

      public async Task<short> GetLowestTimezoneComponentIdAsync(CancellationToken ct = default)
      {
            return (short)await ComponentHelper.LowestUnassignedNumberAsync<Timezone>(
                  context,
                  x => x.component_id,
                  10,
                  ct
                  );
      }

      public async Task<Pagination<TimezoneDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Timezones.AsNoTracking().AsQueryable();

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

            if (param.locationId >= 0)
            {
                  query = query.Where(x => x.location_id == param.locationId || x.location_id == 1);
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
             .Select(x => new TimezoneDto(
                  x.id,
                  x.component_id,
                  x.name,
                  x.mode,
                  x.active,
                  x.deactive,
                  x.intervals.Select(x => new IntervalDto(
                        x.id,
                        x.component_id,
                        new DaysInWeekDto(
                              x.days.id,
                              x.days.component_id,
                              x.days.sunday,
                              x.days.monday,
                              x.days.tuesday,
                              x.days.wednesday,
                              x.days.thursday,
                              x.days.friday,
                              x.days.saturday,
                              x.location_id
                              ),
                        x.days_detail,
                        x.start,
                        x.end,
                        x.location_id,
                        x.is_active
                  )).ToList(),
                  x.location_id,
                  x.is_active
                  ))
            .ToListAsync(ct);

            return new Pagination<TimezoneDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }
}