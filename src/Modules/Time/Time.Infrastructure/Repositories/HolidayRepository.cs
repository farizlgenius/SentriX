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
      public async Task AddAsync(Holiday domain, CancellationToken ct = default)
      {
           await context.Holidays.AddAsync(
                  new Persistences.Entities.Holiday(
                        domain.Guid,
                        domain.ComponentId,
                        domain.Name,
                        domain.Start,
                        domain.End,
                        domain.LocationId,
                        domain.IsActive,
                        domain.IsDefault
                        )
            );

            await context.SaveChangesAsync(ct);
            
      }

      public async Task DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var data = await context.Holidays
            .OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .FirstOrDefaultAsync();

            if(data is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            context.Holidays.Remove(data);

            await context.SaveChangesAsync(ct);
      }

      public async Task<HolidayDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Holidays.AsNoTracking()
            .OrderByDescending(x => x.id)
            .Where(x => x.guid == guid)
            .Select(x => new HolidayDto(
                  x.guid,
                  x.component_id,
                  x.name,
                  x.start,
                  x.end,
                  x.location_id,
                  x.is_active,
                  x.is_default
            ))
            .FirstOrDefaultAsync() ?? new HolidayDto();
            
      }

      public async Task<int> GetLowestHolidayComponentIdAsync(CancellationToken ct = default)
      {
            return await ComponentHelper.LowestUnassignedNumberAsync<Persistences.Entities.Holiday>
            (
                  context,
                  x => x.component_id,
                  255,
                  ct
            );
      }

      public async Task<Pagination<HolidayDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Holidays.AsNoTracking().Where(x => x.location_id == param.locationId || x.location_id == 0).AsQueryable();

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
                                  EF.Functions.ILike(x.start.ToString(), pattern) ||
                                  EF.Functions.ILike(x.end.ToString(), pattern) 
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.start.ToString().Contains(search) ||
                                  x.end.ToString().Contains(search) 
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
            .Select(x => new HolidayDto(
                  x.guid,
                  x.component_id,
                  x.name,
                  x.start,
                  x.end,
                  x.location_id,
                  x.is_active,
                  x.is_default
            ))
            .ToListAsync(ct);

            return new Pagination<HolidayDto>(param.pageNumber, param.pageSize, totalItems,
            (int)Math.Ceiling(totalItems / (double)param.pageSize)
            , items);
      }

      public async Task<bool> IsAnyByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Holidays.AsNoTracking().AnyAsync(x => x.guid == guid);
      }

      public async Task UpdateAsync(Holiday domain, CancellationToken ct = default)
      {
            var entity = await context.Holidays
            .Where(x => x.guid == domain.Guid)
            .OrderByDescending(x => x.id)
            .FirstOrDefaultAsync(ct);

            if(entity is null)
                  throw new Exception(MessageHelper.DB.RecordNotFound);

            entity.Update(domain);

            context.Holidays.Update(entity);
            await context.SaveChangesAsync(ct);
      }

            public async Task<int> CountHolidayByLocationIdAsync(int location_id, CancellationToken ct = default)
      {
            return await context.Holidays.AsNoTracking().Where(x => x.location_id == location_id).CountAsync();
      }
}