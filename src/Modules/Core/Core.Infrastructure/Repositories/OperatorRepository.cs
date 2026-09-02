using Core.Application.Interfaces;
using Core.Contract.DTOs.Operator;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class OperatorRepository(CoreDbContext context) : IOperatorRepository
{
  public async Task AddAsync(Operator entity, CancellationToken ct = default)
  {
    await context.Operators.AddAsync(
      new Persistences.Entities.Operator(entity)
      , ct);

    await context.SaveChangesAsync(ct);
  }

  public async Task AddOperatorLocationsAsync(int operatorId, int locationId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Operators
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Operator, guid.ToString());

    context.Operators.Remove(entity);

    await context.SaveChangesAsync(ct);

  }


  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Operators
      .Where(x => guids.Contains(x.guid))
      .ToListAsync(ct);

    context.Operators.RemoveRange(entities);

    await context.SaveChangesAsync(ct);

  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Operators
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Operator, guid.ToString());

    entity.is_active = false;

    context.Operators.Update(entity);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Operators
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Operator, guid.ToString());

    entity.is_active = true;

    context.Operators.Update(entity);

    await context.SaveChangesAsync(ct);

    return true;
  }


  public async Task<OperatorDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Operators
    .AsNoTracking()
    .OrderByDescending(x => x.id)
      .Where(x => x.guid == guid)
      .Select(x => new OperatorDto(
        x.guid,
        x.username,
        x.title,
        x.firstname,
        x.middlename,
        x.lastname,
        x.gender,
        x.email,
        x.phone,
        x.role.guid,
        x.role.name,
        x.joined_date,
        x.expired_date ?? new DateTime(9999, 01, 01, 0, 0, 0, DateTimeKind.Utc),
        x.operator_locations.Select(x => x.location.guid).ToList(),
        x.is_active,
        x.is_default
      ))
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Operator, guid.ToString());


  }

  public async Task<IEnumerable<OperatorDto>> GetByLocationAsync(int locationId, CancellationToken ct = default)
  {
    return await context.Operators
    .AsNoTracking()
    .OrderByDescending(x => x.id)
      .Where(x => x.operator_locations.Any(x => x.location_id == locationId))
      .Select(x => new OperatorDto(
        x.guid,
        x.username,
        x.title,
        x.firstname,
        x.middlename,
        x.lastname,
        x.gender,
        x.email,
        x.phone,
        x.role.guid,
        x.role.name,
        x.joined_date,
        x.expired_date ?? new DateTime(9999, 01, 01, 0, 0, 0, DateTimeKind.Utc),
        x.operator_locations.Select(x => x.location.guid).ToList(),
        x.is_active,
        x.is_default
      ))
      .ToArrayAsync(ct);
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Operators
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Operator, guid.ToString());

    return entity.id;
  }

  public async Task<IEnumerable<Guid>> GetLocationGuidsByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.OperatorLocations
      .AsNoTracking()
      .Where(x => x.@operator.username.Equals(username))
      .Select(x => x.location.guid)
      .ToArrayAsync(ct);
  }



  public async Task<OperatorDto> GetOperatorByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Operators
         .AsNoTracking()
         .OrderByDescending(x => x.id)
         .Where(x => x.username.Equals(username))
         .Select(x => new OperatorDto(
        x.guid,
        x.username,
        x.title,
        x.firstname,
        x.middlename,
        x.lastname,
        x.gender,
        x.email,
        x.phone,
        x.role.guid,
        x.role.name,
        x.joined_date,
        x.expired_date ?? new DateTime(9999, 01, 01, 0, 0, 0, DateTimeKind.Utc),
        x.operator_locations.Select(x => x.location.guid).ToList(),
        x.is_active,
        x.is_default
      ))
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Operator, username);


  }


  public async Task<Pagination<OperatorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Operators
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
              EF.Functions.ILike(x.username, pattern) ||
              EF.Functions.ILike(x.email, pattern) ||
              EF.Functions.ILike(x.firstname, pattern) ||
              EF.Functions.ILike(x.lastname, pattern) ||
              EF.Functions.ILike(x.middlename, pattern) ||
              EF.Functions.ILike(x.phone, pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.username.Contains(search) ||
              x.email.Contains(search) ||
              x.firstname.Contains(search) ||
              x.lastname.Contains(search) ||
              x.middlename.Contains(search) ||
              x.phone.Contains(search)
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
          .Select(x => new OperatorDto(
        x.guid,
        x.username,
        x.title,
        x.firstname,
        x.middlename,
        x.lastname,
        x.gender,
        x.email,
        x.phone,
        x.role.guid,
        x.role.name,
        x.joined_date,
        x.expired_date ?? new DateTime(9999, 01, 01, 0, 0, 0, DateTimeKind.Utc),
        x.operator_locations.Select(x => x.location.guid).ToList(),
        x.is_active,
        x.is_default
      )).ToListAsync();

    return new Pagination<OperatorDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<string> GetPassowrdByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .Where(x => x.username.Equals(username))
      .Select(x => x.password)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Operator, username);

  }

  public async Task<Guid> GetRoleGuidByUsernameAsync(string username, CancellationToken ct = default)
  {
    var res = await context.Operators
      .AsNoTracking()
      .Where(x => x.username.Equals(username))
      .Select(x => x.role.guid)
      .FirstOrDefaultAsync(ct);

    if (res == Guid.Empty)
      throw new NotFoundException($"No role available for {EntityType.User}:{username}");

    return res;
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = 0, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnyEmailAsync(string email, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.email.Equals(email), ct);
  }

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid, ct);
  }

  public async Task<bool> IsAnyUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.username.Equals(username), ct);
  }

  public async Task<bool> IsAnyWithLocationIdAsync(int locationId, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.operator_locations.Any(ol => ol.location_id == locationId), ct);
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.guid == guid && x.is_default, ct);
  }

  public async Task<bool> IsLocationIdsValidAsync(List<int> LocationIds, CancellationToken ct = default)
  {
    return await context.Locations
      .AsNoTracking()
      .Where(x => LocationIds.Contains(x.id))
      .Select(x => x.id)
      .Distinct()
      .CountAsync() == LocationIds.Count;
  }

  public async Task<bool> IsOperatorExistsByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.username.Equals(username), ct);
  }

  public async Task<bool> IsValidRoleIdAsync(int RoleId, CancellationToken ct = default)
  {
    return await context.Roles
      .AsNoTracking()
      .AnyAsync(x => x.id == RoleId, ct);

  }

  public async Task RemoveOperatorLocationByLocationIdAsync(int locationId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task RemoveOperatorLocationsAsync(int locationId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task UpdateAsync(Operator entity, CancellationToken ct = default)
  {
    var existingEntity = await context.Operators
      .Where(x => x.guid == entity.Guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.Operator, entity.Guid.ToString());

    existingEntity.username = entity.Username;
    existingEntity.title = entity.Title;
    existingEntity.firstname = entity.Firstname;
    existingEntity.middlename = entity.Middlename;
    existingEntity.lastname = entity.Lastname;
    existingEntity.gender = entity.Gender;
    existingEntity.email = entity.Email;
    existingEntity.phone = entity.Phone;
    existingEntity.role_id = entity.RoleId;

    var existingLocationIds = existingEntity.operator_locations.Select(x => x.location_id).ToList();

    var newLocationIds = entity.LocationIds;

    foreach (var locationId in existingLocationIds)
    {
      if (!newLocationIds.Contains(locationId))
      {
        var operatorLocation = existingEntity.operator_locations.FirstOrDefault(x => x.location_id == locationId);
        if (operatorLocation != null)
        {
          existingEntity.operator_locations.Remove(operatorLocation);
        }
      }
    }

    foreach (var locationId in newLocationIds)
    {
      if (!existingLocationIds.Contains(locationId))
      {
        existingEntity.operator_locations.Add(new Persistences.Entities.OperatorLocation(0, locationId));
      }
    }


    context.Operators.Update(existingEntity);

    await context.SaveChangesAsync(ct);
  }
}