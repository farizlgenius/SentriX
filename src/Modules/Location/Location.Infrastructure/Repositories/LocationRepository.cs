using System;
using Location.Application.Interfaces;
using Location.Contract.DTOs;
using Location.Contract.Events;
using Location.Domain.Entities;
using Location.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Location.Infrastructure.Repositories;

public class LocationRepository(LocationDbContext context,IMessageBus bus) : ILocationRepository
{
      public async Task<LocationDto> AddAsync(Locations location)
      {
            var entity = new Persistences.Entities.Locations(location);
            var data = await context.Locations.AddAsync(entity);
            var save = await context.SaveChangesAsync();
            if (data is null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            // // 💥 DB GENERATED ID IS READY HERE
            // entity.RaiseDomainEvent(
            //     new LocationCreatedDomainEvent(
            //         entity.id,
            //         entity.name));

            // // 🔥 Publish domain events
            // foreach (var domainEvent in entity.DomainEvents)
            //       await mediator.Publish(domainEvent);

            // entity.ClearDomainEvents();

            await bus.PublishAsync(new LocationCreatedEvent(
                data.Entity.id
            ));


            return new LocationDto(
              data.Entity.id,
              data.Entity.name,
              data.Entity.description,
              data.Entity.country_id,
            await context.Countries.AsNoTracking().OrderByDescending(c => c.id).Where(c => c.id == data.Entity.country_id).Select(c => c.name).FirstOrDefaultAsync() ?? "");

      }

      public async Task<LocationDto> DeleteByIdAsync(int id)
      {
            var entity = await context.Locations.OrderByDescending(l => l.id).FirstOrDefaultAsync(l => l.id == id);
            if (entity is null)
                  throw new NotFoundException(MessageHelper.DB.RecordNotFound);
            var data = context.Locations.Remove(entity);
            var save = await context.SaveChangesAsync();
            if (data is null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);

            // // 💥 DB GENERATED ID IS READY HERE
            // entity.RaiseDomainEvent(
            //     new LocationDeletedDomainEvent(
            //         entity.id,
            //         entity.name));

            // // 🔥 Publish domain events
            // foreach (var domainEvent in entity.DomainEvents)
            //       await mediator.Publish(domainEvent);

            // entity.ClearDomainEvents();

            await bus.PublishAsync(
                  new LocationDeletedEvent(
                        data.Entity.id
                   )
            );

            return new LocationDto(
          data.Entity.id,
          data.Entity.name,
          data.Entity.description,
          data.Entity.country_id,
        await context.Countries.AsNoTracking().OrderByDescending(c => c.id).Where(c => c.id == data.Entity.country_id).Select(c => c.name).FirstOrDefaultAsync() ?? "");

      }

      public async Task<List<LocationDto>> DeleteRangeAsync(List<int> ids)
      {

            var records = await context.Locations.Where(l => ids.Contains(l.id)).ToListAsync();
            if (records is null || records.Count == 0)
                  throw new NotFoundException(MessageHelper.DB.RecordNotFound);

            context.Locations.RemoveRange(records);
            var save = await context.SaveChangesAsync();
            if (save <= 0)
                  throw new Exception(MessageHelper.DB.DeleteRecordUnsuccessful);

            return records.Select(data => new LocationDto(
              data.id,
              data.name,
              data.description,
              data.country_id,
            context.Countries.AsNoTracking().OrderByDescending(c => c.id).Where(c => c.id == data.country_id).Select(c => c.name).FirstOrDefault() ?? "")).ToList();


      }

      public async Task<List<CountryDto>> GetAllCountriesAsync()
      {
            return await context.Countries
                .AsNoTracking()
                .Select(x => new CountryDto(x.id, x.name, x.code))
                .ToListAsync();
      }

      public async Task<List<LocationDto>> GetAsync()
      {
            return await context.Locations
            .AsNoTracking()
            .Select(x => new LocationDto(x.id, x.name, x.description, x.country_id, x.country.name))
            .ToListAsync();

      }

      public async Task<Pagination<CountryDto>> GetCountriesPaginationAsync(int Page, int PageSize, string Search)
      {
            var query = context.Countries.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(Search))
            {
                  var search = Search.Trim();

                  if (context.Database.IsNpgsql())
                  {
                        var pattern = $"%{search}%";

                        query = query.Where(x =>
                            EF.Functions.ILike(x.name, pattern) ||
                            EF.Functions.ILike(x.code, pattern)
                        );
                  }
                  else // SQL Server
                  {
                        query = query.Where(x =>
                            x.name.Contains(search) ||
                            x.code.Contains(search)
                        );
                  }
            }

            var totalItems = await query.CountAsync();
            var items = await query
            .OrderByDescending(x => x.id)
            .Skip((Page - 1) * PageSize)
            .Take(PageSize)
            .Select(x => new CountryDto(x.id, x.name, x.code))
            .ToListAsync();

            return new Pagination<CountryDto>(Page, PageSize, totalItems, (int)Math.Ceiling(totalItems / (double)PageSize), items);
      }

      public async Task<string> GetNameByIdAsync(int id, CancellationToken ct = default)
      {
            return await context.Locations.AsNoTracking()
            .Where(x => x.id == id)
            .Select(x => x.name)
            .FirstOrDefaultAsync() ?? string.Empty;
      }

      public async Task<Pagination<LocationDto>> GetPaginationAsync(int Page, int PageSize, string Search)
      {
            var query = context.Locations.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                  var search = Search.Trim();

                  if (context.Database.IsNpgsql())
                  {
                        var pattern = $"%{search}%";

                        query = query.Where(x =>
                            EF.Functions.ILike(x.name, pattern) ||
                            EF.Functions.ILike(x.description, pattern) ||
                            EF.Functions.ILike(x.country.name, pattern)
                        );
                  }
                  else // SQL Server
                  {
                        query = query.Where(x =>
                            x.name.Contains(search) ||
                            x.description.Contains(search) ||
                            x.country.name.Contains(search)
                        );
                  }
            }
            var totalItems = await query.CountAsync();
            var items = await query
            .OrderByDescending(x => x.id)
            .Skip((Page - 1) * PageSize)
            .Take(PageSize)
            .Select(x => new LocationDto(x.id, x.name, x.description, x.country_id, x.country.name))
            .ToListAsync();

            return new Pagination<LocationDto>(Page, PageSize, totalItems, (int)Math.Ceiling(totalItems / (double)PageSize), items);
      }

      public async Task<List<LocationDto>> GetRangeLocationAsync(List<int> ids)
      {
            var locations = await context.Locations
                .AsNoTracking()
                .OrderBy(l => l.id)
                .Where(l => ids.Contains(l.id))
                .Select(x => new LocationDto(x.id, x.name, x.description, x.country_id, x.country.name))
                .ToListAsync();

            return locations;
      }

      public async Task<bool> IsAllExistByIdsAsync(List<int> ids)
      {
            var count = await context.Locations
                .AsNoTracking()
                .Where(l => ids.Contains(l.id))
                .CountAsync();

            return count == ids.Count;
      }

      public async Task<bool> IsAnyByIdAsync(int id,CancellationToken ct = default)
      {
            return await context.Locations.AsNoTracking()
            .AnyAsync(l => l.id == id, ct);
      }

      public async Task<bool> IsAnyNameAsync(string name)
      {
            return await context.Locations.AsNoTracking()
            .AnyAsync(l => l.name.ToLower().Equals(name.Trim().ToLower()));
      }

      public async Task<bool> IsLocationIdsValidAsync(List<int> LocationIds, CancellationToken ct = default)
      {
            return await context.Locations.AsNoTracking().AnyAsync(x => LocationIds.Contains(x.id), ct);
      }

      public async Task<bool> IsValidCountryAsync(int id)
      {
            return await context.Countries.AsNoTracking()
            .AnyAsync(c => c.id == id);
      }

      public async Task<LocationDto> UpdateAsync(Locations location)
      {
            var record = await context.Locations.OrderByDescending(l => l.id).FirstOrDefaultAsync(l => l.id == location.Id);
            if (record is null)
                  throw new NotFoundException(MessageHelper.DB.RecordNotFound);

            record.Update(location);

            var data = context.Locations.Update(record);
            var save = await context.SaveChangesAsync();
            if (data is null || save <= 0)
                  throw new Exception(MessageHelper.DB.SaveRecordUnsuccessful);


            return new LocationDto(
          data.Entity.id,
          data.Entity.name,
          data.Entity.description,
          data.Entity.country_id,
        await context.Countries.AsNoTracking().OrderByDescending(c => c.id).Where(c => c.id == data.Entity.country_id).Select(c => c.name).FirstOrDefaultAsync() ?? "");


      }
}
