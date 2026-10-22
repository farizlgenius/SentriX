using Core.Application.Interfaces;
using Core.Contract.DTOs.Action;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class ActionRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Action,Domain.Entities.Actions,ActionDto>(
      context,
      d => new Persistences.Entities.Action(d),
      (en,dom) =>
      {
            en.name = dom.Name;
            en.device_id = dom.DeviceId;
            en.type = dom.Type;
            en.metadata = dom.Metadata;
            en.updated_at = DateTime.UtcNow;
      },
      ActionProjections.ToDto,
      ActionProjections.ToCommand
),IActionRepository
{
    

      public async Task<Pagination<ActionDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
          return await base.GetPaginationAsync(
            param,
            param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
            [x => x.name,x => x.type.ToString()],
            ct
          );
      }

   

      public async Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Action
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.procedure_actions.Any(),ct);
      }


}