using Core.Application.Interfaces;
using Core.Contract.DTOs.Location;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class LocationRepository(CoreDbContext context) : ILocationRepository
{
      public async Task AddAsync(Core.Domain.Entities.Location entity, CancellationToken ct = default)
      {
            await context.Database.BeginTransactionAsync(ct);
            try
            {
                  var location = new Persistences.Entities.Location(entity);

                  location.user_locations = new List<UserLocation>
                  {
                        new UserLocation(1)
                  };
                  var data = await context.Locations.AddAsync(location, ct);


                  await context.SaveChangesAsync(ct);

                  await context.Database.CommitTransactionAsync(ct);
            }
            catch
            {
                  await context.Database.RollbackTransactionAsync(ct);
                  throw;
            }

      }

      public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
      {
            var entity = await context.Locations
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct);

            context.Locations.Remove(entity ?? throw new NotFoundException(EntityType.Location, guid.ToString()));

            await context.SaveChangesAsync(ct);
      }

      public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            var entities = await context.Locations
                  .Where(x => guids.Contains(x.guid) && x.is_default == false)
                  .ToArrayAsync(ct);

            context.Locations.RemoveRange(entities);

            await context.SaveChangesAsync(ct);
      }

      public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Locations
                  .Where(x => x.guid == guid)
                  .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Location, guid.ToString());

            en.is_active = false;

            context.Locations.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
      {
            var en = await context.Locations
                   .Where(x => x.guid == guid)
                   .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Location, guid.ToString());

            en.is_active = true;

            context.Locations.Update(en);

            await context.SaveChangesAsync(ct);

            return true;
      }

      public async Task<LocationDto> GetAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Locations
                  .AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => new LocationDto(
                        x.guid,
                        x.name,
                        x.description,
                        x.country_id,
                        x.country.name,
                        x.is_active,
                        x.is_default
                  )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Location, guid.ToString());
      }


      public async Task<IEnumerable<LocationDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
      {
            return await context.Locations
                   .AsNoTracking()
                   .Where(x => x.id == locationId)
                   .Select(x => new LocationDto(
                         x.guid,
                         x.name,
                         x.description,
                         x.country_id,
                         x.country.name,
                         x.is_active,
                         x.is_default
                   )).ToListAsync();
      }

      public async Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken ct = default)
      {
            return await context.Countries
                  .AsNoTracking()
                  .Select(x => new CountryDto(
                        x.id,
                        x.name,
                        x.code
                  ))
                  .ToArrayAsync();
      }

      public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await context.Locations.AsNoTracking()
                  .Where(x => x.guid == guid)
                  .Select(x => x.id)
                  .FirstOrDefaultAsync();

            if (res == 0)
                  throw new NotFoundException(EntityType.Location, guid.ToString());

            return res;
      }

      public async Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await context.Locations
                  .AsNoTracking()
                  .Where(x => guids.Contains(x.guid))
                  .OrderByDescending(x => x.id)
                  .Select(x => x.id)
                  .ToListAsync();
      }

      public async Task<IEnumerable<LocationDto>> GetListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            return await context.Locations
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
                  )).ToListAsync();
      }

      public async Task<Pagination<LocationDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            var query = context.Locations
                  .AsNoTracking()
                  .AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.search))
            {
                  if (!string.IsNullOrWhiteSpace(param.search))
                  {
                        var search = param.search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.description, pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.description.Contains(search)
                              );
                        }

                  }
            }


            if (param.startDate != null)
            {
                  var startUtc = DateTime.SpecifyKind(param.startDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.created_at >= startUtc);
            }

            if (param.endDate != null)
            {
                  var endUtc = DateTime.SpecifyKind(param.endDate.Value, DateTimeKind.Utc);
                  query = query.Where(x => x.created_at <= endUtc);
            }

            var count = await query.CountAsync();

            var res = await query
                  .AsNoTracking()
                  .OrderByDescending(e => e.created_at)
                  .Skip((param.pageNumber - 1) * param.pageSize)
                  .Take(param.pageSize)
                  .Select(e => new LocationDto(
                        e.guid,
                        e.name,
                        e.description,
                        e.country_id,
                        e.country.name,
                        e.is_active,
                        e.is_default
                  )).ToListAsync();

            return new Pagination<LocationDto>(
                  param.pageNumber,
                  param.pageSize,
                  count,
                  (int)Math.Ceiling(count / (double)param.pageSize),
                  res
                  );
      }

      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = default, CancellationToken ct = default)
      {
            return await context.Locations
                  .AsNoTracking()
                  .AnyAsync(x => x.name.Equals(name));
      }

      public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Locations
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid);
      }

      public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
      {
            return await context.Locations
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.is_default);
      }

      public async Task UpdateAsync(Core.Domain.Entities.Location entity, CancellationToken ct = default)
      {
            var en = await context.Locations
                  .Where(x => x.guid == entity.Guid)
                  .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Location, entity.Guid.ToString());

            en.name = entity.Name;
            en.description = entity.Description;
            en.country_id = entity.CountryId;

            context.Locations.Update(en);

            await context.SaveChangesAsync(ct);
      }
}