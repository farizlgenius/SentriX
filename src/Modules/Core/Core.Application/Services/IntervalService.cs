using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class IntervalService(IIntervalRepository repo, IMessageBus bus) : IInterval
{
  public async Task<Guid> CreateAsync(CreateIntervalDto dto, CancellationToken ct = default)
  {
    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    var d = new Domain.Entities.Interval(
      dto.Start,
      dto.End,
      new DayInWeek(
        dto.Days.Sunday,
        dto.Days.Monday,
        dto.Days.Tuesday,
        dto.Days.Wednesday,
        dto.Days.Thursday,
        dto.Days.Friday,
        dto.Days.Saturday
      ),
      locationId
    );

    if (await repo.IsAnySameDataSetAsync(d.StartTime, d.EndTime, d.Days))
      throw new DuplicateException(EntityType.Interval, "Interval Data");

    // Send command to controller

    await repo.AddAsync(d, ct);

    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.Interval, guid.ToString());

    // Check relation

    // Send command

    await repo.DeleteAsync(guid);

    return true;
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new NotFoundException(EntityType.Role);

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.Role, guid.ToString());

      // Check relate object here

    }

    await repo.DeleteRangeAsync(guids);

    return guids;
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Role, guid.ToString());

    return await repo.DisableAsync(guid, ct);
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Role, guid.ToString());

    return await repo.EnableAsync(guid, ct);
  }

  public async Task<IntervalDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<IEnumerable<IntervalDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(guid));
    return await repo.GetByLocationAsync(locationId, ct);
  }

  public async Task<Pagination<IntervalDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateIntervalDto dto, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.TimeZone, dto.Guid.ToString());

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));


    var d = new Domain.Entities.Interval(
     dto.Start,
     dto.End,
     new DayInWeek(
       dto.Days.Sunday,
       dto.Days.Monday,
       dto.Days.Tuesday,
       dto.Days.Wednesday,
       dto.Days.Thursday,
       dto.Days.Friday,
       dto.Days.Saturday
     ),
     locationId
   );

    if (await repo.IsAnySameDataSetAsync(d.StartTime, d.EndTime, d.Days))
      throw new DuplicateException(EntityType.Interval, "Interval Data");

    // Send command to controller

    await repo.UpdateAsync(d, ct);

    return d.Guid;
  }
}