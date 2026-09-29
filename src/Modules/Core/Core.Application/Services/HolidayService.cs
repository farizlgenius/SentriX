using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class HolidayService(
  IHolidayRepository repo,
  IMessageBus bus
) : IHoliday
{
  public async Task<Guid> CreateAsync(CreateHolidayDto dto, CancellationToken ct = default)
  {
    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId))
      throw new DuplicateException(EntityType.TimeZone.ToString(), dto.Name);

    var d = new Domain.Entities.Holiday(
      dto.Name,
      dto.Start,
      dto.End,
      locationId
    );

    // Send command to controller

    await repo.AddAsync(d, ct);

    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.TimeZone.ToString(), guid.ToString());

    // Check relation

    // Send command

    await repo.DeleteAsync(guid);

    return true;
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new NotFoundException(EntityType.Role.ToString());

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.Role.ToString(), guid.ToString());

      // Check relate object here

    }

    await repo.DeleteRangeAsync(guids);

    return guids;
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Role.ToString(), guid.ToString());

    return await repo.DisableAsync(guid, ct);
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Role.ToString(), guid.ToString());

    return await repo.EnableAsync(guid, ct);
  }

  public async Task<HolidayDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<IEnumerable<HolidayDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetByLocationAsync(guid, ct);
  }

  public async Task<Pagination<HolidayDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateHolidayDto dto, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.TimeZone.ToString(), dto.Guid.ToString());

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

    if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId))
      throw new DuplicateException(EntityType.TimeZone.ToString(), dto.Name);

    var d = new Domain.Entities.Holiday(
      dto.Guid,
      dto.Name,
      dto.Start,
      dto.End,
      locationId
    );

    // Send command to controller

    await repo.UpdateAsync(d, ct);

    return d.Guid;
  }

      public Task UploadAsync(CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}