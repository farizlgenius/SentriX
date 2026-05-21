using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using Time.Application.Interfaces;
using Time.Contract.DTOs;
using Time.Domain.Entities;
using Time.Infrastructure.Persistences;

namespace Time.Infrastructure.Repositories;

public sealed class HolidayRepository(TimeDbContext context) : IHolidayRepository
{
      public async Task<HolidayDto> CreateHolidayAsync(Holiday domain, CancellationToken ct = default)
      {
            var data = await context.Holidays.AddAsync(
                  new Persistences.Entities.Holiday(
                        domain.ComponentId,
                        domain.Name,
                        domain.Year,
                        domain.Month,
                        domain.Day,
                        domain.Metadata,
                        domain.LocationId,
                        domain.IsActive
                        )
            );

            var save = await context.SaveChangesAsync(ct);

            if(data.Entity == null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            return new HolidayDto(
                  data.Entity.id,
                  data.Entity.component_id,
                  data.Entity.name,
                  data.Entity.year,
                  data.Entity.month,
                  data.Entity.day,
                  data.Entity.metadata,
                  data.Entity.location_id,
                  data.Entity.is_active
            );

            
      }

      public async Task<HolidayDto> DeleteByIdAsync(int id, CancellationToken ct = default)
      {
            var data = await context.Holidays.OrderByDescending(x => x.id)
            .Where(x => x.id == id)
            .FirstOrDefaultAsync();

            if(data == null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            var res = context.Holidays.Remove(data);
            return new HolidayDto(
                  res.Entity.id,
                  res.Entity.component_id,
                  res.Entity.name,
                  res.Entity.year,
                  res.Entity.month,
                  res.Entity.day,
                  res.Entity.metadata,
                  res.Entity.location_id,
                  res.Entity.is_active
                  );
      }

      public async Task<HolidayDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Holidays.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.id == id)
            .Select(x => new HolidayDto(
                  x.id,
                  x.component_id,
                  x.name,
                  x.year,
                  x.month,
                  x.day,
                  x.metadata,
                  x.location_id,
                  x.is_active
            ))
            .FirstOrDefaultAsync() ?? new HolidayDto(
                  0,
                  0,
                  string.Empty,
                  0,
                  0,
                  0,
                  string.Empty,
                  0,
                  false
                  );
            
      }

      public async Task<int> GetLowestHolidayComponentIdAsync(CancellationToken ct = default)
      {
            return await ComponentHelper.LowestUnassignedNumberAsync<Persistences.Entities.Holiday>
            (
                  context,
                  x => x.component_id,
                  10,
                  ct
            );
      }

      public async Task<Pagination<HolidayDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Holidays.AsNoTracking().AsQueryable();

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
                                  EF.Functions.ILike(x.year.ToString(), pattern) ||
                                  EF.Functions.ILike(x.month.ToString(), pattern) ||
                                  EF.Functions.ILike(x.day.ToString(), pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.year.ToString().Contains(search) ||
                                  x.month.ToString().Contains(search) ||
                                  x.day.ToString().Contains(search) 
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
            .Select(r => new HolidayDto(
                  r.id,
                  r.component_id,
                  r.name,
                  r.year,
                  r.month,
                  r.day,
                  r.metadata,
                  r.location_id,
                  r.is_active
            ))
            .ToListAsync(ct);

            return new Pagination<HolidayDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }
}