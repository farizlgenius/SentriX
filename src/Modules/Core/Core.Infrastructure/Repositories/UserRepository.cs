using Core.Application.Interfaces;
using Core.Contract.DTOs.User;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class UserRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.User,Domain.Entities.User,UserDto>(
  context,
  d => new Persistences.Entities.User(d),
  (en,dom) =>{},
  UserProjections.ToDto,
  UserProjections.ToCommand
),IUserRepository
{


  public async Task ChangePasswordAsync(string username, string hashed, CancellationToken ct = default)
  {
    var entity = await Context.Users
      .Where(x => x.username != null && x.username.Equals(username))
      .FirstOrDefaultAsync(cancellationToken: ct) ?? throw new NotFoundException(EntityType.USER.ToString(), username);

    entity.password = hashed;
    entity.updated_at = DateTime.UtcNow;

    Context.Users.Update(entity);

    await Context.SaveChangesAsync(ct);
  }

  public async Task<UserDto> GetByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await Context.Users
      .AsNoTracking()
      .OrderByDescending(x => x.id)
      .Where(x => x.username != null && x.username.Equals(username))
      .Select(x => new UserDto(
        x.guid,
        x.user_code,
        x.username ?? string.Empty,
        x.identification,
        x.title,
        x.firstname,
        x.middlename,
        x.lastname,
        x.gender,
        x.date_of_birth,
        x.email,
        x.phone,
        x.role == null ? string.Empty : x.role.name,
        x.company == null ? string.Empty : x.company.name,
        x.department == null ? string.Empty : x.department.name,
        x.position == null ? string.Empty : x.position.name,
        x.address,
        x.active_time,
        x.expire_time,
        x.additionals.Select(x => x.additional).ToList(),
        x.user_groups.Select(x => x.group.name).ToList(),
        x.cards.Select(c => new CardDto(
          c.bits,
          c.fac,
          c.card_number
        )).ToList(),
        new LicensePlateDto(x.license_plate == null ? string.Empty : x.license_plate.license_plate),
        new PinDto(x.pin == null ? string.Empty : x.pin.pin),
        new QrCodeDto(x.qr_code == null ? string.Empty : x.qr_code.qr_code),
        x.user_locations.Select(x => x.location.name).ToList(),
        x.metadata
      )).FirstOrDefaultAsync(cancellationToken: ct) ?? throw new NotFoundException(EntityType.USER.ToString(), username);
  }

  public async Task<Guid> GetDefaultLocationGuidAsync()
  {
    throw new NotImplementedException();
  }

  public async Task<string> GetHashByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await Context.Users
      .AsNoTracking()
      .Where(x => x.username != null && x.username.Equals(username))
      .Select(x => x.password)
      .FirstOrDefaultAsync(cancellationToken: ct) ?? throw new NotFoundException(EntityType.OPERATOR.ToString(), username);
  }



  public async Task<IEnumerable<Guid>> GetLocationGuidByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await Context.Users
      .AsNoTracking()
      .Where(x => x.username != null && x.username.Equals(username))
      .SelectMany(x => x.user_locations.Select(x => x.location.guid))
      .ToArrayAsync(cancellationToken: ct);
  }


  public async Task<Pagination<UserDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = Context.Users
                  .AsNoTracking()
                  .Where(x => x.user_locations.Any(x => x.location.guid == param.locationGuid))
                  .AsQueryable();

    if (!string.IsNullOrWhiteSpace(param.search))
    {
      if (!string.IsNullOrWhiteSpace(param.search))
      {
        var search = param.search.Trim();

        if (Context.Database.IsNpgsql())
        {
          var pattern = $"%{search}%";

          query = query.Where(x =>
              (x.username != null ? EF.Functions.ILike(x.username, pattern) : false) ||
              EF.Functions.ILike(x.user_code, pattern) ||
              EF.Functions.ILike(x.identification, pattern) ||
              EF.Functions.ILike(x.title.ToString(), pattern) ||
              EF.Functions.ILike(x.firstname, pattern) ||
              EF.Functions.ILike(x.middlename, pattern) ||
              EF.Functions.ILike(x.lastname, pattern) ||
              EF.Functions.ILike(x.gender.ToString(), pattern) ||
              EF.Functions.ILike(x.email, pattern) ||
              EF.Functions.ILike(x.phone, pattern) ||
              (x.company != null && EF.Functions.ILike(x.company.name, pattern)) ||
              (x.department != null && EF.Functions.ILike(x.department.name, pattern)) ||
              (x.position != null && EF.Functions.ILike(x.position.name, pattern)) ||
              EF.Functions.ILike(x.address, pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              (x.username != null && x.username.Contains(search)) ||
              x.user_code.Contains(search) ||
              x.identification.Contains(search) ||
              x.title.ToString().Contains(search) ||
              x.firstname.Contains(search) ||
              x.middlename.Contains(search) ||
              x.lastname.Contains(search) ||
              x.gender.ToString().Contains(search) ||
              x.email.Contains(search) ||
              x.phone.Contains(search) ||
              (x.company != null && x.company.name.Contains(search)) ||
              (x.department != null && x.department.name.Contains(search)) ||
              (x.position != null && x.position.name.Contains(search)) ||
              x.address.Contains(search)
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
          .Select(e => new UserDto(
            e.guid,
            e.user_code,
            e.username ?? string.Empty,
            e.identification,
            e.title,
            e.firstname,
            e.middlename,
            e.lastname,
            e.gender,
            e.date_of_birth,
            e.email,
            e.phone,
            e.role != null ? e.role.name : string.Empty,
            e.company != null ? e.company.name : string.Empty,
            e.department != null ? e.department.name : string.Empty,
            e.position != null ? e.position.name : string.Empty,
            e.address,
            e.active_time,
            e.expire_time,
            e.additionals.Select(x => x.additional).ToList(),
            e.user_groups.Select(x => x.group.name).ToList(),
            e.cards.Select(x => new CardDto(
              x.bits,
              x.fac,
              x.card_number
            )).ToList(),
            new LicensePlateDto(e.license_plate == null ? string.Empty : e.license_plate.license_plate),
        new PinDto(e.pin == null ? string.Empty : e.pin.pin),
        new QrCodeDto(e.qr_code == null ? string.Empty : e.qr_code.qr_code),
            e.user_locations.Select(x => x.location.name).ToList(),
            e.metadata
          )).ToListAsync();

    return new Pagination<UserDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = default, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

 

  public async Task<bool> IsAnyUsernameAsync(string username, CancellationToken ct = default)
  {
    return await Context.Users
      .AsNoTracking()
      .AnyAsync(x => x.username != null && x.username.Equals(username), cancellationToken: ct);
  }



  public override async Task UpdateAsync(User user, CancellationToken ct = default)
  {


    try
    {
      await Context.Database.BeginTransactionAsync(ct);

      var entity = await Context.Users
      .Where(x => x.guid == user.Guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.USER.ToString(), user.Guid.ToString());


      // Mapping the properties from the domain entity to the persistence entity
      entity.title = user.Title;
      entity.firstname = user.FirstName;
      entity.middlename = user.MiddleName;
      entity.lastname = user.LastName;
      entity.gender = user.Gender;
      entity.date_of_birth = user.DateOfBirth;
      entity.email = user.Email;
      entity.phone = user.Phone;
      entity.address = user.Address;
      entity.active_time = user.JoinedTime;
      entity.expire_time = user.ExpiredTime;

      // Company
      entity.company_id = user.CompanyId;

      // Department 
      entity.department_id = user.DepartmentId;

      // Position
      entity.position_id = user.PositionId;

      // Additional

      var existingAdditionals = await Context.UserAdditionals
        .Where(x => x.user_id == entity.id)
        .ToListAsync(ct);

      var incomingAdditionals = user.Additionals.Select(a => a.ToLower()).ToHashSet();

      foreach (var item in existingAdditionals)
      {
        if (!incomingAdditionals.Contains(item.additional.ToLower()))
        {
          Context.UserAdditionals.Remove(item);
        }
      }

      var existingValueAdditional = existingAdditionals.Select(a => a.additional.ToLower()).ToHashSet();

      foreach (var additional in incomingAdditionals)
      {

        if (!existingValueAdditional.Contains(additional.ToLower()))
        {

          Context.UserAdditionals.Add(
              new Persistences.Entities.UserAdditional
              {
                user_id = entity.id,
                additional = additional
              });
        }
      }

      // Card 

      var existingCards = await Context.Cards
        .Where(x => x.user_id == entity.id)
        .ToListAsync(ct);

      var incomingCards = user.Cards.Select(c => new Card(c.Bits, c.Fac, c.Fac)).ToHashSet();

      foreach (var item in existingCards)
      {
        if (!incomingCards.Any(c => c.Bits == item.bits && c.Fac == item.fac && c.CardNumber == item.card_number))
        {
          Context.Cards.Remove(item);
        }
      }

      var existingValueCards = existingCards.Select(c => new Card(c.bits, c.fac, c.card_number)).ToHashSet();

      foreach (var card in incomingCards)
      {
        if (!existingValueCards.Any(c => c.Bits == card.Bits && c.Fac == card.Fac && c.CardNumber == card.CardNumber))
        {
          Context.Cards.Add(
              new Persistences.Entities.Card(card)
              {
                user_id = entity.id,
                bits = card.Bits,
                fac = card.Fac,
                card_number = card.CardNumber
              });
        }
      }

      entity.license_plate = user.LicensePlate is null ? null : new Persistences.Entities.LicensePlate(user.LicensePlate);
      entity.pin = user.Pin is null ? null : new Persistences.Entities.Pin(user.Pin);
      entity.qr_code = user.QrCode is null ? null : new Persistences.Entities.QrCode(user.QrCode);

      // User Locations
      var existingUserLocations = await Context.UserLocations
        .Where(x => x.user_id == entity.id)
        .ToListAsync(ct);

      var incomingUserLocationIds = user.LocationIds.Select(l => l).ToHashSet();

      var existingUserLocationIds = existingUserLocations.Select(l => l.location_id).ToHashSet();

      // Remove Relationships that are not in the incoming list
      foreach (var existingLocation in existingUserLocations)
      {
        if (!incomingUserLocationIds.Contains(existingLocation.location_id))
        {
          Context.UserLocations.Remove(existingLocation);
        }
      }

      // Add new Relationships that are not in the existing list
      foreach (var incomingLocationId in incomingUserLocationIds)
      {
        if (!existingUserLocationIds.Contains(incomingLocationId))
        {
          Context.UserLocations.Add(
              new Persistences.Entities.UserLocation
              {
                user_id = entity.id,
                location_id = incomingLocationId
              });
        }
      }

      // User Groups

      var existingUserGroups = await Context.UserGroups
        .Where(x => x.user_id == entity.id)
        .ToListAsync(ct);

      var incomingUserGroupIds = user.GroupIds.Select(g => g).ToHashSet();

      var existingUserGroupIds = existingUserGroups.Select(g => g.group_id).ToHashSet();

      // Remove Relationships that are not in the incoming list
      foreach (var existingGroup in existingUserGroups)
      {
        if (!incomingUserGroupIds.Contains(existingGroup.group_id))
        {
          Context.UserGroups.Remove(existingGroup);
        }
      }

      // Add new Relationships that are not in the existing list
      foreach (var incomingGroupId in incomingUserGroupIds)
      {
        if (!existingUserGroupIds.Contains(incomingGroupId))
        {
          Context.UserGroups.Add(
              new Persistences.Entities.UserGroup
              {
                user_id = entity.id,
                group_id = incomingGroupId
              });
        }
      }

      Context.Users.Update(entity);

      await Context.SaveChangesAsync(ct);

      await Context.Database.CommitTransactionAsync(ct);
    }
    catch
    {
      await Context.Database.RollbackTransactionAsync(ct);
      throw;
    }

  }



  

  public async Task<bool> IsAnyIdentificationAsync(string identification, CancellationToken ct = default)
  {
    return await Context.Users
      .AsNoTracking()
      .AnyAsync(x => x.identification.Equals(identification), cancellationToken: ct);
  }


  public async Task UpdateImagePathAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await Context.Users
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.USER.ToString(), guid.ToString());

    entity.face = new Persistences.Entities.Face(guid);

    Context.Users.Update(entity);
    await Context.SaveChangesAsync(ct);

  }

  public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }



      public async Task<IEnumerable<(string mac, string ip, Vendor vendor,short externalId)>> GetDetailsByUserGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await Context.Users
              .AsNoTracking()
              .Where(x => x.guid == guid)
              .SelectMany(
                x => x.user_groups.SelectMany(
                g => g.group.components.Select(c => new
                {
                    c.door.device.mac,
                    c.door.device.ip,
                    c.door.device.vendor,
                    c.door.device.external_id
                })
              )).ToArrayAsync(ct);

            return [.. res.Select(x => (x.mac,x.ip,x.vendor,x.external_id ?? -1))];
      }

      public Task<Guid> GetRoleGuidByUsernameAsync(string username, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}