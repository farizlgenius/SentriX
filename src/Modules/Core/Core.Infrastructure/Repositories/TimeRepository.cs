using Core.Application.Interfaces;
using Core.Contract.DTOs.Device;
using Core.Contract.DTOs.Time;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Persistences.Entities;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;

using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class TimeRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.TimeZone, Domain.Entities.TimeZone, TimeZoneDto>(
  context,
  d => new Persistences.Entities.TimeZone(d),
  (en, dom) => { },
  TimeProjections.ToDto,
  TimeProjections.ToCommand
), ITimeRepository
{


  public async Task<Components> GetComponentsAsync(Guid locationGuid, DateTime syncedAt, CancellationToken ct = default)
  {
    // Executes a single SQL query using SUM(CASE WHEN...) to get all counts
    var stats = await Context.TimeZones
        .Where(x => x.is_default || x.location.guid == locationGuid)
        .GroupBy(x => 1) // Group all matching rows into a single bucket
        .Select(g => new
        {
          Total = g.Count(),
          Uploaded = g.Count(x => x.updated_at <= syncedAt)
        })
        .FirstOrDefaultAsync(ct);

    // If no records match, stats will be null, so we default to 0
    var total = stats?.Total ?? 0;
    var uploaded = stats?.Uploaded ?? 0;

    // Calculate remain in memory to save the database an extra calculation
    var remain = total - uploaded;

    return new Components(
        EntityType.TIMEZONE,
        total,
        uploaded,
        remain,
        remain == 0
    );
  }

      public async Task<string> GetNameByExternalIdAsync(short externalId, CancellationToken ct = default)
      {
            return await Context.TimeZones
              .AsNoTracking()
              .Where(x => x.external_id == externalId)
              .Select(x => x.name)
              .FirstOrDefaultAsync(ct) ?? "Unknown";
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    return await Context.TimeZones
      .AsNoTracking()
      .Where(x => x.location.guid == guid || x.is_default)
      .Select(x => new OptionDto(
        x.name,
        x.guid,
        string.Empty
      )).ToArrayAsync();
  }

  public async Task<Pagination<TimeZoneDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await base.GetPaginationAsync(
      param,
      param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
      [x => x.name]
      ,ct);
  }

  public async Task<Dictionary<Guid, int>> GetTimeZoneIdsMapGuidsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    return await Context.TimeZones
      .AsNoTracking()
      .Where(x => guids.Contains(x.guid))
      .ToDictionaryAsync(x => x.guid, x => x.id, ct);
  }



  public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }



  public override async Task UpdateAsync(Domain.Entities.TimeZone entity, CancellationToken ct = default)
  {

    try
    {
      await Context.Database.BeginTransactionAsync(ct);

      var en = await Context.TimeZones
      .Where(x => x.guid == entity.Guid)
      .OrderByDescending(x => x.id)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.TIMEZONE.ToString(), entity.Guid.ToString());

      var existingInterval = await Context.TimeZoneIntervals.Where(x => x.timezone_id == en.id).ToArrayAsync();

      var removes = new List<Persistences.Entities.TimeZoneInterval>();

      var existingIds = new List<int>();

      foreach (var exiting in existingInterval)
      {
        if (!entity.IntervalIds.Contains(exiting.id))
        {
          removes.Add(exiting);
          continue;
        }

        existingIds.Add(exiting.id);

      }

      Context.TimeZoneIntervals.RemoveRange(removes);

      var newInterval = entity.IntervalIds
      .Where(x => !existingIds.Contains(x))
      .Select(x => new Persistences.Entities.TimeZoneInterval(0, x))
      .ToList();

      await Context.TimeZoneIntervals.AddRangeAsync(newInterval, ct);

      en.name = entity.Name;
      en.location_id = entity.LocationId;

      await Context.SaveChangesAsync(ct);

      await Context.Database.CommitTransactionAsync(ct);

    }
    catch
    {
      await Context.Database.RollbackTransactionAsync(ct);
      throw;
    }

  }
}