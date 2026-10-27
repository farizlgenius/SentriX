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
            try
            {
                  await Context.Database.BeginTransactionAsync(ct);

                  var entity = await Context.Areas.AddAsync(
                        new Persistences.Entities.Area(domain),ct
                  );

                  foreach(var (doorId, antipassback, area, direction, type) in domain.DoorsIn)
                  {
                        var en = await Context.Doors.Where(x => x.id == doorId).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.DOOR.ToString());

                        en.from_area_id = area;
                        en.antipassback_from = antipassback;
                        en.updated_at = DateTime.UtcNow;

                        Context.Doors.Update(en);

                  }

                  foreach(var (doorId, antipassback, area, direction, type) in domain.DoorsOut)
                  {
                        var en = await Context.Doors.Where(x => x.id == doorId).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.DOOR.ToString());

                        en.to_area_id = entity.Entity.id;
                        en.antipassback_to = antipassback;
                        en.updated_at = DateTime.UtcNow;

                        Context.Doors.Update(en);

                  }

                  await Context.SaveChangesAsync(ct);

                  await Context.Database.CommitTransactionAsync(ct);
            }
            catch
            {
                  await Context.Database.RollbackTransactionAsync(ct);
                  throw;
            }
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
            try
            {
                  await Context.Database.BeginTransactionAsync(ct);

                  var entity = await Context.Areas.Where(x => x.guid == dom.Guid).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.AREA.ToString(),dom.Guid.ToString());

                  entity.name = dom.Name;
                  entity.multi_occ_mode = dom.MultiOccMode;
                  entity.area_mode = dom.AreaMode;
                  entity.occ_max = dom.OccMax;
                  entity.occ_down = dom.OccDown;
                  entity.occ_up = dom.OccUp;
                  entity.metadata = dom.Metadata;
                  entity.updated_at = DateTime.UtcNow;

                  Context.Areas.Update(entity);

                  foreach(var (doorId, antipassback, area, direction, type) in dom.DoorsIn)
                  {
                        var en = await Context.Doors.Where(x => x.id == doorId).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.DOOR.ToString());

                        en.from_area_id = area;
                        en.antipassback_from = antipassback;
                        en.updated_at = DateTime.UtcNow;

                        Context.Doors.Update(en);

                  }

                  foreach(var (doorId, antipassback, area, direction, type) in dom.DoorsOut)
                  {
                        var en = await Context.Doors.Where(x => x.id == doorId).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.DOOR.ToString());

                        en.to_area_id = area;
                        en.antipassback_to = antipassback;
                        en.updated_at = DateTime.UtcNow;

                        Context.Doors.Update(en);

                  }

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