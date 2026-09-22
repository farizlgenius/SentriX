using Core.Application.Interfaces;
using Core.Contract.DTOs.Events.AdapterEvent;
using Core.Contract.DTOs.Events.Event;
using Core.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class EventRepostory(CoreDbContext context) : IEventRepository
{
  public async Task AddAdapterAsync(Domain.Entities.AdapterEvent @event, CancellationToken ct = default)
  {
    await context.AdapterEvents.AddAsync(
new AdapterEvent(@event)
, ct);

    await context.SaveChangesAsync(ct);
  }

  public async Task AddAsync(Domain.Entities.Event entity, CancellationToken ct = default)
  {
    await context.Events.AddAsync(
      new Event(entity)
      , ct);

    await context.SaveChangesAsync(ct);
  }

  public async Task AddExceptionAsync(string path, string exception, string innerException, string stackTrace, CancellationToken ct = default)
  {
    await context.ExceptionEvent.AddAsync(
      new Persistences.Entities.ExceptionEvent(
        path,
        exception,
        innerException,
        stackTrace
      ),
      ct
    );

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

  public async Task<Pagination<AdapterEventDto>> GetAdapterPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.AdapterEvents
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
              EF.Functions.ILike(x.name.ToString(), pattern) ||
              EF.Functions.ILike(x.mac, pattern) ||
              EF.Functions.ILike(x.command, pattern) ||
              EF.Functions.ILike(x.tag.ToString(), pattern) ||
              EF.Functions.ILike(x.body, pattern) ||
              EF.Functions.ILike(x.status.ToString(), pattern) ||
              EF.Functions.ILike(x.reason, pattern) ||
              EF.Functions.ILike(x.response, pattern) ||
              EF.Functions.ILike(x.vendor.ToString(), pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.name.ToString().Contains(search) ||
              x.mac.Contains(search) ||
              x.command.Contains(search) ||
              x.tag.ToString().Contains(search) ||
              x.body.Contains(search) ||
              x.status.ToString().Contains(search) ||
              x.reason.Contains(search) ||
              x.response.Contains(search) ||
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
         .Select(x => new AdapterEventDto(
          x.guid,
          x.name,
          x.mac,
          x.component_id,
          x.command,
          x.tag,
          x.send_at,
          x.received_at,
          x.body,
          x.status,
          x.reason,
          x.response,
          x.vendor,
          x.location == null ? Guid.Empty : x.location.guid,
          x.is_active,
          x.is_default
      )).ToListAsync();

    return new Pagination<AdapterEventDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
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

  public Task<Guid> GetGuidByIdAsync(int id, CancellationToken ct = default)
  {
    throw new NotImplementedException();
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

  public async Task UpdateAdapterEventStatusAsync(int componentId, int tag, CommandStatus status, string reason, CancellationToken ct = default)
  {
    var entity = await context.AdapterEvents
      .Where(x => x.component_id == componentId && x.tag == tag)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.AdapterEvent, $"Tag: {tag}");

    entity.reason = reason;
    entity.status = status;
    entity.received_at = DateTime.UtcNow;

  }

  public async Task UpdateAsync(Domain.Entities.Event entity, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}