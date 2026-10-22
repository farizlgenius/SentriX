using Core.Application.Interfaces;
using Core.Contract.DTOs.Door;
using Core.Contract.DTOs.Turnstile;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class TurnstileRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Turnstile,Domain.Entities.Turnstile,TurnstileDto>(
  context,
  d => new Persistences.Entities.Turnstile(d),
  (en,dom) =>
  {
    en.name = dom.Name;
    context.Lanes.RemoveRange(en.lanes);
    en.lanes.Clear();

    en.lanes = dom.Lanes.Select(x => new Persistences.Entities.Lane(x)).ToList();
    en.updated_at = DateTime.UtcNow;
  },
  TurnstileProjections.ToDto,
  TurnstileProjections.ToCommand
),ITurnstileRepository
{
 


 

  

      public async Task<Pagination<TurnstileDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await base.GetPaginationAsync(
      param,
      param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name],
      ct
    );
  }

     

  public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

 


}