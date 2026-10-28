using Core.Application.Interfaces;
using Core.Contract.DTOs.Area;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class AreaRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Area, Domain.Entities.Area, AreaDto>(
      context,
      d => new Persistences.Entities.Area(d),
      (en, dom) =>
      {
            en.name = dom.Name;
            en.multi_occ_mode = dom.MultiOccMode;
            en.area_mode = dom.AreaMode;
            en.occ_max = dom.OccMax;
            en.occ_down = dom.OccDown;
            en.occ_up = dom.OccUp;
            en.metadata = dom.Metadata;
            en.updated_at = DateTime.UtcNow;
      },
      AreaProjections.ToDto,
      AreaProjections.ToCommand
), IAreaRepository
{
      public override async Task AddAsync(Domain.Entities.Area domain, CancellationToken ct = default)
      {
            await using var transaction = await Context.Database.BeginTransactionAsync(ct);
            try
            {
                  // 1. Persist the new Area first so that entity.Entity.id is generated
                  var entity = await Context.Areas.AddAsync(new Persistences.Entities.Area(domain), ct);
                  await Context.SaveChangesAsync(ct);

                  var newAreaId = entity.Entity.id;

                  // 2. Pre-fetch all involved doors in a single round-trip to avoid N+1 queries and tracking issues
                  var inDoorIds = domain.DoorsIn.Select(d => d.doorId).ToList();
                  var outDoorIds = domain.DoorsOut.Select(d => d.doorId).ToList();
                  var allDoorIds = inDoorIds.Concat(outDoorIds).Distinct().ToList();

                  var existingDoors = await Context.Doors
                      .Where(x => allDoorIds.Contains(x.id))
                      .ToDictionaryAsync(x => x.id, ct);

                  // 3. Process Doors entering the area
                  foreach (var (doorId, antipassback, fromAreaId, direction, type) in domain.DoorsIn)
                  {
                        if (!existingDoors.TryGetValue(doorId, out var door))
                              throw new NotFoundException(EntityType.DOOR.ToString());

                        // Door enters the new area:
                        door.from_area_id = fromAreaId;      // Verify fromAreaId exists or is nullable
                        door.to_area_id = newAreaId;         // Target is the new area
                        door.antipassback_from = antipassback;
                        door.updated_at = DateTime.UtcNow;
                  }

                  // 4. Process Doors leaving the area
                  foreach (var (doorId, antipassback, toAreaId, direction, type) in domain.DoorsOut)
                  {
                        if (!existingDoors.TryGetValue(doorId, out var door))
                              throw new NotFoundException(EntityType.DOOR.ToString());

                        // Door exits the new area:
                        if(door.type != DoorType.Dual)
                        {
                              door.from_area_id = newAreaId;       // Origin is the new area
                              door.to_area_id = entity.Entity.id;          // Destination area
                        }
                        door.antipassback_to = antipassback;
                        door.updated_at = DateTime.UtcNow;
                  }

                  // 5. Commit updates
                  await Context.SaveChangesAsync(ct);
                  await transaction.CommitAsync(ct);
            }
            catch
            {
                  await Context.Database.RollbackTransactionAsync(ct);
                  throw;
            }
      }

      public async Task<Dictionary<Guid, int>> GetAreaIdMapGuidByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await Context.Areas
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .ToDictionaryAsync(x => x.guid, x => x.id, ct);
      }

      public async Task<Pagination<AreaDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location == null || x.location.guid == param.locationGuid : null,
                  [x => x.name],
                  ct
            );
      }

      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public override async Task UpdateAsync(Domain.Entities.Area dom, CancellationToken ct = default)
      {
            await using var transaction = await Context.Database.BeginTransactionAsync(ct);
            try
            {

                  var entity = await Context.Areas.Where(x => x.guid == dom.Guid).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.AREA.ToString(), dom.Guid.ToString());

                  entity.name = dom.Name;
                  entity.multi_occ_mode = dom.MultiOccMode;
                  entity.area_mode = dom.AreaMode;
                  entity.occ_max = dom.OccMax;
                  entity.occ_down = dom.OccDown;
                  entity.occ_up = dom.OccUp;
                  entity.metadata = dom.Metadata;
                  entity.updated_at = DateTime.UtcNow;

                  Context.Areas.Update(entity);

                  // 2. Pre-fetch all involved doors in a single round-trip to avoid N+1 queries and tracking issues
                  var inDoorIds = dom.DoorsIn.Select(d => d.doorId).ToList();
                  var outDoorIds = dom.DoorsOut.Select(d => d.doorId).ToList();
                  var allDoorIds = inDoorIds.Concat(outDoorIds).Distinct().ToList();

                  var existingDoors = await Context.Doors
                      .Where(x => allDoorIds.Contains(x.id))
                      .ToDictionaryAsync(x => x.id, ct);

                  // 3. Process Doors entering the area
                  foreach (var (doorId, antipassback, fromAreaId, direction, type) in dom.DoorsIn)
                  {
                        if (!existingDoors.TryGetValue(doorId, out var door))
                              throw new NotFoundException(EntityType.DOOR.ToString());

                        // Door enters the new area:
                        door.from_area_id = fromAreaId;      // Verify fromAreaId exists or is nullable
                        door.to_area_id = entity.id;         // Target is the new area
                        door.antipassback_from = antipassback;
                        door.updated_at = DateTime.UtcNow;
                  }

                  // 4. Process Doors leaving the area
                  foreach (var (doorId, antipassback, toAreaId, direction, type) in dom.DoorsOut)
                  {
                        if (!existingDoors.TryGetValue(doorId, out var door))
                              throw new NotFoundException(EntityType.DOOR.ToString());

                        // Door exits the new area:
                        door.from_area_id = entity.id;       // Origin is the new area
                        door.to_area_id = toAreaId;          // Destination area
                        door.antipassback_to = antipassback;
                        door.updated_at = DateTime.UtcNow;
                  }

                  // 5. Commit updates
                  await Context.SaveChangesAsync(ct);
                  await transaction.CommitAsync(ct);
            }
            catch
            {
                  await Context.Database.RollbackTransactionAsync(ct);
                  throw;
            }
      }
}