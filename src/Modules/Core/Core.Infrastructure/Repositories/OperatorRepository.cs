using Core.Application.Interfaces;
using Core.Contract.DTOs.Operator;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class OperatorRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Operator, Domain.Entities.Operator, OperatorDto>(
  context,
  d => new Persistences.Entities.Operator(d),
  (en, dom) =>
  {
    en.username = dom.Username;
    en.title = dom.Title;
    en.firstname = dom.Firstname;
    en.middlename = dom.Middlename;
    en.lastname = dom.Lastname;
    en.gender = dom.Gender;
    en.email = dom.Email;
    en.phone = dom.Phone;
    en.role_id = dom.RoleId;
    en.updated_at = DateTime.UtcNow;

    var existingLocationIds = en.operator_locations.Select(x => x.location_id).ToList();

    var newLocationIds = dom.LocationIds;

    foreach (var locationId in existingLocationIds)
    {
      if (!newLocationIds.Contains(locationId))
      {
        var operatorLocation = en.operator_locations.FirstOrDefault(x => x.location_id == locationId);
        if (operatorLocation != null)
        {
          en.operator_locations.Remove(operatorLocation);
        }
      }
    }

    foreach (var locationId in newLocationIds)
    {
      if (!existingLocationIds.Contains(locationId))
      {
        en.operator_locations.Add(new Persistences.Entities.OperatorLocation(0, locationId));
      }
    }
  },
  OperatorProjections.ToDto,
  OperatorProjections.ToCommand
), IOperatorRepository
{


  public async Task AddOperatorLocationsAsync(int operatorId, int locationId, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }









  public async Task<IEnumerable<Guid>> GetLocationGuidsByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await Context.OperatorLocations
      .AsNoTracking()
      .Where(x => x.@operator.username.Equals(username))
      .Select(x => x.location.guid)
      .ToArrayAsync(ct);
  }



  public async Task<OperatorDto> GetOperatorByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await Context.Operators
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
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.OPERATOR.ToString(), username);


  }

  public async Task<Pagination<OperatorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await base.GetPaginationAsync(
      param,
      null,
      [
        x => x.username,
        x => x.email,
        x => x.firstname,
        x => x.middlename,
        x => x.lastname,
        x => x.phone
      ],
      ct
    );
  }


  

  public async Task<string> GetPassowrdByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await Context.Operators
      .AsNoTracking()
      .Where(x => x.username.Equals(username))
      .Select(x => x.password)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.OPERATOR.ToString(), username);

  }

  public async Task<Guid> GetRoleGuidByUsernameAsync(string username, CancellationToken ct = default)
  {
    var res = await Context.Operators
      .AsNoTracking()
      .Where(x => x.username.Equals(username))
      .Select(x => x.role.guid)
      .FirstOrDefaultAsync(ct);

    if (res == Guid.Empty)
      throw new NotFoundException($"No role available for {EntityType.USER}:{username}");

    return res;
  }





  public async Task<bool> IsAnyEmailAsync(string email, CancellationToken ct = default)
  {
    return await Context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.email.Equals(email), ct);
  }



  public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> IsAnyUsernameAsync(string username, CancellationToken ct = default)
  {
    return await Context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.username.Equals(username), ct);
  }

  public async Task<bool> IsAnyWithLocationIdAsync(int locationId, CancellationToken ct = default)
  {
    return await Context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.operator_locations.Any(ol => ol.location_id == locationId), ct);
  }





  public async Task<bool> IsLocationIdsValidAsync(List<int> LocationIds, CancellationToken ct = default)
  {
    return await Context.Locations
      .AsNoTracking()
      .Where(x => LocationIds.Contains(x.id))
      .Select(x => x.id)
      .Distinct()
      .CountAsync(cancellationToken: ct) == LocationIds.Count;
  }

  public async Task<bool> IsOperatorExistsByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await Context.Operators
      .AsNoTracking()
      .AnyAsync(x => x.username.Equals(username), ct);
  }

  public async Task<bool> IsValidRoleIdAsync(int RoleId, CancellationToken ct = default)
  {
    return await Context.Roles
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


}