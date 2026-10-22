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
            en.procedure_actions = dom.ActionIds.Select(x => new ProcedureAction(x)).ToList();
            en.updated_at = DateTime.UtcNow;
      },
      ProcedureProjections.ToDto,
      ProcedureProjections.ToCommand
),IProcedureRepository
{

      public async Task<Pagination<ProcedureDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name],
                  ct);
      }


      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }


}