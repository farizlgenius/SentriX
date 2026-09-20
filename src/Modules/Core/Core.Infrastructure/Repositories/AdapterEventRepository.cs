using Core.Application.Interfaces;
using Core.Contract.DTOs.AdapterEvent;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Core.Infrastructure.Repositories;

public sealed class AdapterEventRepository(CoreDbContext context) : IAdapterEventRepository
{
  public async Task AddAsync(AdapterEvent entity, CancellationToken ct = default)
  {
    await context.AdapterEvents.AddAsync(
      new Persistences.Entities.AdapterEvent(entity)
      , ct);
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

  public async Task<AdapterEventDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<AdapterEventDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    return await context.AdapterEvents
      .AsNoTracking()
      .Where(x => x.location_id == locationId)
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
        x.location.guid,
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

  public async Task<Pagination<AdapterEventDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.AdapterEvents
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
              EF.Functions.ILike(x.mac, pattern) ||
              EF.Functions.ILike(x.command, pattern) ||
              EF.Functions.ILike(x.tag.ToString(), pattern) ||
              EF.Functions.ILike(x.send_at.ToString(), pattern) ||
              EF.Functions.ILike(x.received_at.ToString(), pattern) ||
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
              x.send_at.ToString().Contains(search) ||
              x.received_at.ToString().Contains(search) ||
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
        x.location.guid,
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

  public async Task UpdateAsync(AdapterEvent entity, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}