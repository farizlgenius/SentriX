using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class DoorRepository(CoreDbContext context) : IDoorRepository
{
  public async Task AddAsync(Door entity, CancellationToken ct = default)
  {
    await context.Doors.AddAsync(
      new Persistences.Entities.Door(entity)
      , ct);

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Doors
    .OrderByDescending(x => x.id)
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Door, guid.ToString());

    context.Doors.Remove(entity);

    await context.SaveChangesAsync(ct);

  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Doors
      .Where(x => guids.Contains(x.guid))
      .ToArrayAsync();

    context.Doors.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Doors
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Door, guid.ToString());

    en.is_active = false;

    context.Doors.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Doors
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Door, guid.ToString());

    en.is_active = true;

    context.Doors.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<DoorDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Doors
      .AsNoTracking()
      .Where(x => new DoorDto(
        x.guid,
        x.name,
        x.metadata,
        x.reader
      ))
  }

  public async Task<IEnumerable<DoorDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<DoorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task UpdateAsync(Door entity, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}