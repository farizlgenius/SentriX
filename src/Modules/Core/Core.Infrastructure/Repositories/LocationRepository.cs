using Core.Application.Interfaces;
using Core.Contract.DTOs.Location;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Persistences.Entities;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class LocationRepository(CoreDbContext context) :BaseRepository<Persistences.Entities.Location,Domain.Entities.Location,LocationDto>(
      context,
      d => new Persistences.Entities.Location(d),
      (en,dom) =>
      {
            en.name = dom.Name;
            en.description = dom.Description;
            en.country_id = dom.CountryId;
            en.updated_at = DateTime.UtcNow;
      },
      LocationProjections.ToDto,
      LocationProjections.ToCommand
),ILocationRepository
{
      public override async Task AddAsync(Core.Domain.Entities.Location entity, CancellationToken ct = default)
      {
            await Context.Database.BeginTransactionAsync(ct);
            try
            {
                  var location = new Persistences.Entities.Location(entity);

                  location.operator_locations = new List<OperatorLocation>
                  {
                        new(1,0)
                  };


                  var change = AuditHelper.GetEntityChanges(await Context.Locations.AddAsync(location, ct));



                  await Context.SaveChangesAsync(ct);

                  await Context.Database.CommitTransactionAsync(ct);
            }
            catch
            {
                  await Context.Database.RollbackTransactionAsync(ct);
                  throw;
            }

      }

     
      public async Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken ct = default)
      {
            return await Context.Countries
                  .AsNoTracking()
                  .Select(x => new CountryDto(
                        x.id,
                        x.name,
                        x.code
                  ))
                  .ToArrayAsync(ct);
      }

    

      public async Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await Context.Locations
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .OrderByDescending(x => x.id)
                  .Select(x => x.id)
                  .ToListAsync(ct);
      }

      public async Task<IEnumerable<LocationDto>> GetListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await Context.Locations
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .OrderBy(x => x.id)
                  .Select(e => new LocationDto(
                        e.guid,
                        e.name,
                        e.description,
                        e.country_id,
                        e.country.name,
                        e.is_active,
                        e.is_default
                  )).ToListAsync(ct);
      }

   
      public async Task<Pagination<LocationDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
           return await base.GetPaginationAsync(
            param,
            param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.guid == param.locationGuid : null,
            [x => x.name,x => x.description],
            ct
           );
      }



      public async Task<IEnumerable<Guid>> IsAnyInvalidGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await Context.Locations
                  .AsNoTracking()
                  .Where(x => !guids.Contains(x.guid))
                  .Select(x => x.guid)
                  .ToListAsync(ct);
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

}