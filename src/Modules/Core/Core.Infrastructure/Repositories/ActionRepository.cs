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
      public async Task<IEnumerable<(int id, ActionType type,short? externalId,string? metadata,int sequence)>> GetActionDetailByGuidAsync(IEnumerable<(Guid guid,int sequence)> actions, CancellationToken ct = default)
      {
            var guids = actions.Select(x => x.guid);
            var res = await Context.Action
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .Select(x => new
                  {
                        x.id,
                        x.guid,
                        x.type,
                        x.external_id,
                        x.metadata
                  }).ToArrayAsync(ct);

            return [.. res.Select(x => (x.id,x.type,externalId:x.external_id,x.metadata,actions.Where(a => a.guid == x.guid).Select(s => s.sequence).FirstOrDefault()))];
      }

      public async Task<IEnumerable<OptionDto>> GetActionOptionByDeviceAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Action
                  .AsNoTracking()
                  .Where(x => x.device.guid == guid)
                  .Select(x => new OptionDto(
                        x.name,
                        x.guid,
                        x.type
                  )).ToArrayAsync(ct);
      }

      public async Task<IEnumerable<OptionDto>> GetActionOptionByTypeAndDeviceAsync(Guid guid, ActionType type, CancellationToken ct = default)
      {
            return await Context.Action
                  .AsNoTracking()
                  .Where(x => x.device.guid == guid && x.type == type)
                  .Select(x => new OptionDto(
                        x.name,
                        x.guid,
                        x.device.mac
                  )).ToArrayAsync(ct);
      }

      public async Task<Pagination<ActionDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
          return await base.GetPaginationAsync(
            param,
            param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location == null || x.location.guid == param.locationGuid : null,
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