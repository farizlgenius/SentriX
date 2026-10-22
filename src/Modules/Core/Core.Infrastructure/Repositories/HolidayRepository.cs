using Core.Application.Interfaces;
using Core.Contract.DTOs.Time;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class HolidayRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Holiday,Domain.Entities.Holiday,HolidayDto>(
  context,
  d => new Persistences.Entities.Holiday(d),
  (en,dom) =>
  {
    en.name = dom.Name;
    en.start = dom.Start;
    en.end = dom.End;
    en.location_id = dom.LocationId;
    en.updated_at = DateTime.UtcNow;
  },
  HolidayProjections.ToDto,
  HolidayProjections.ToCommand
),IHolidayRepository
{
 
 

      public async Task<Pagination<HolidayDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await base.GetPaginationAsync(
      param,
      param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
      [x => x.name],
      ct
    );
  }

     

      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
  {
    return await Context.Holidays
      .AsNoTracking()
      .AnyAsync(x => x.name.Equals(name) && x.location_id == locationId, cancellationToken: ct);
  }

 

  public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

 
}