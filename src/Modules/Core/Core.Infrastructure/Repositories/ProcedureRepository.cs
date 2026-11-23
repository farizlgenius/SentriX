using Core.Application.Interfaces;
using Core.Contract.DTOs.Procedure;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Persistences.Entities;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class ProcedureRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Procedure,Domain.Entities.Procedure,ProcedureDto>(
      context,
      d => new Persistences.Entities.Procedure(d),
      (en,dom) =>
      {
            en.device_id = dom.DeviceId;
            en.updated_at = DateTime.UtcNow;
      },
      ProcedureProjections.ToDto,
      ProcedureProjections.ToCommand
),IProcedureRepository
{
      public async Task<short> GetFreeExternalIdByDeviceMacAsync(string mac, int max, short[] exception, CancellationToken ct = default)
      {
            var existingIds = await Context.Procedures
                  .AsNoTracking()
                  .Where(x => x.device.mac == mac)
                  .Select(x => x.external_id!.Value)
                  .ToArrayAsync(ct);

            var existIdsHash = existingIds.ToHashSet();

            if (exception != null)
            {
                  existIdsHash.UnionWith(exception);
            }

            for (short id = 1; id < max; id++)
            {
                  if (!existIdsHash.Contains(id))
                        return id;
            }

            throw new ExceedException(EntityType.PROCEDURE.ToString());
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Procedures
                  .AsNoTracking()
                  .Where(x => x.device.guid == guid)
                  .Select(x => new OptionDto(
                        x.name,
                        x.guid.ToString(),
                        string.Empty
                  )).ToArrayAsync(ct);
      }

      public async Task<Pagination<ProcedureDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name],
                  ct);
      }


      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Procedures
                  .AsNoTracking()
                  .AnyAsync(x => x.triggers.Any(),ct);
      }

      public override async Task UpdateAsync(Domain.Entities.Procedure d,CancellationToken ct  = default)
      {
            await using var transaction = await Context.Database.BeginTransactionAsync(ct);
            try
            {

                  var entity = await Context.Procedures.Where(x => x.guid == d.Guid).FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.PROCEDURE.ToString(), d.Guid.ToString());

                  entity.name = d.Name;
                  entity.device_id = d.DeviceId;
                  entity.updated_at = DateTime.UtcNow;

                  Context.Action.RemoveRange(entity.actions);

                  entity.actions.Clear();

                  entity.actions = [.. d.Actions.Select(x => new Persistences.Entities.Action(x))];

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