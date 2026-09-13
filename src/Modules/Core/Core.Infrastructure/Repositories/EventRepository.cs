using Core.Application.Interfaces;
using Core.Contract.DTOs.Event;
using Core.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class EventRepostory(CoreDbContext context) : IEventRepository
{
  public async Task AddAsync(Domain.Entities.Event entity, CancellationToken ct = default)
  {
    await context.AddAsync(
      new Event(entity)
      , ct);

    await context.SaveChangesAsync(ct);
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

  public async Task<EventDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<EventDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    return await context.Events
      .AsNoTracking()
      .Where(x => x.location_id == locationId)
      .Select(x => new EventDto(
        x.guid,
        x.timestamp,
        x.actor,
        x.module,
        x.event_type,
        x.image_name,
        x.mac,
        x.component_name,
        x.event_code,
        x.remarks,
        x.capture_image_name,
        x.vendor,
        x.location == null ? Guid.Empty : x.location.guid,
        x.is_active,
        x.is_default
      )).ToArrayAsync();
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<EventDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Events
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
              EF.Functions.ILike(x.timestamp.ToString(), pattern) ||
              EF.Functions.ILike(x.actor, pattern) ||
              EF.Functions.ILike(x.module, pattern) ||
              EF.Functions.ILike(x.event_type, pattern) ||
              EF.Functions.ILike(x.mac, pattern) ||
              EF.Functions.ILike(x.component_name, pattern) ||
              EF.Functions.ILike(x.event_code, pattern) ||
              EF.Functions.ILike(x.remarks, pattern) ||
              EF.Functions.ILike(x.vendor.ToString(), pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.timestamp.ToString().Contains(search) ||
              x.module.Contains(search) ||
              x.event_type.Contains(search) ||
              x.mac.Contains(search) ||
              x.component_name.Contains(search) ||
              x.event_code.Contains(search) ||
              x.remarks.Contains(search) ||
              x.vendor.ToString().Contains(search)
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
         .Select(x => new EventDto(
        x.guid,
        x.timestamp,
        x.actor,
        x.module,
        x.event_type,
        x.image_name,
        x.mac,
        x.component_name,
        x.event_code,
        x.remarks,
        x.capture_image_name,
        x.vendor,
        x.location == null ? Guid.Empty : x.location.guid,
        x.is_active,
        x.is_default
      )).ToListAsync();

    return new Pagination<EventDto>(
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

  public async Task UpdateAsync(Domain.Entities.Event entity, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}