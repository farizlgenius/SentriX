using Core.Application.Interfaces;
using Core.Contract.DTOs.Trigger;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Core.Infrastructure.Repositories;

public sealed class TriggerVariableRepository(
      CoreDbContext context
) : BaseRepository<Persistences.Entities.TriggerVariable, Domain.Entities.TriggerVariable, TriggerVariableDto>(
      context,
      d => new Persistences.Entities.TriggerVariable(d),
      (en,dom) =>
      {
            en.trigger_id = dom.TriggerId;
            en.device_id = dom.DeviceId;
            en.updated_at = DateTime.UtcNow;
      },
      TriggerVariableProjections.ToDto,
      TriggerVariableProjections.ToCommand
), ITriggerVariableRepository
{
      public async Task<Pagination<TriggerVariableDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name]
                  ,ct);
      }

      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.TriggerVariables
                  .AsNoTracking()
                  .AnyAsync(x => x.trigger_variable_map_trigger.Any(),ct);
      }
}