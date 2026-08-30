using Core.Application.Interfaces;
using Core.Contract.DTOs.User;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class UserRepository(CoreDbContext context) : IUserRepository
{
  public async Task AddAsync(User entity, CancellationToken ct = default)
  {
    await context.Users.AddAsync(
      new Persistences.Entities.User(entity), ct
    );

    await context.SaveChangesAsync(ct);
  }

  public async Task ChangePasswordAsync(string username, string hashed, CancellationToken ct = default)
  {
    var entity = await context.Users
      .Where(x => x.username.Equals(username))
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.User, username);

    entity.password = hashed;
    entity.updated_at = DateTime.UtcNow;

    context.Users.Update(entity);

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteAsync(Guid guid, CancellationToken ct = default)
  {
    var entity = await context.Users
      .Where(x => x.guid == guid)
      .FirstOrDefaultAsync(ct);

    context.Users.Remove(entity ?? throw new NotFoundException(EntityType.User, guid.ToString()));

    await context.SaveChangesAsync(ct);
  }

  public async Task DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    var entities = await context.Users
                  .Where(x => guids.Contains(x.guid) && x.is_default == false)
                  .ToArrayAsync(ct);

    context.Users.RemoveRange(entities);

    await context.SaveChangesAsync(ct);
  }

  public async Task<bool> DisableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Users
          .Where(x => x.guid == guid)
          .FirstOrDefaultAsync(ct) ?? throw new NotFoundException(EntityType.User, guid.ToString());

    en.is_active = false;
    en.updated_at = DateTime.UtcNow;

    context.Users.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<bool> EnableAsync(Guid guid, CancellationToken ct = default)
  {
    var en = await context.Users
           .Where(x => x.guid == guid)
           .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.User, guid.ToString());

    en.is_active = true;
    en.updated_at = DateTime.UtcNow;

    context.Users.Update(en);

    await context.SaveChangesAsync(ct);

    return true;
  }

  public async Task<UserDto> GetAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Users
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .Select(x => new UserDto(
        x.guid,
        x.username,
        x.identification,
        x.title,
        x.firstname,
        x.middlename,
        x.lastname,
        x.gender,
        x.date_of_birth,
        x.email,
        x.phone,
        x.is_operator,
        x.is_user,
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
        x.license_plates.Select(
          l => new LicensePlateDto(l.license_plate)
        ).ToList(),
        x.pins.Select(x => new PinDto(x.pin)).ToList(),
        x.qr_codes.Select(x => new QrCodeDto(x.qr_code)).ToList(),
        new FaceDto(x.face == null ? string.Empty : x.face.image_name),
        x.user_locations.Select(x => x.location.name).ToList()
      )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.User, guid.ToString());
  }

  public async Task<UserDto> GetByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Users
      .AsNoTracking()
      .OrderByDescending(x => x.id)
      .Where(x => x.username.Equals(username))
      .Select(x => new UserDto(
        x.guid,
        x.username,
        x.identification,
        x.title,
        x.firstname,
        x.middlename,
        x.lastname,
        x.gender,
        x.date_of_birth,
        x.email,
        x.phone,
        x.is_operator,
        x.is_user,
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
        x.license_plates.Select(
          l => new LicensePlateDto(l.license_plate)
        ).ToList(),
        x.pins.Select(x => new PinDto(x.pin)).ToList(),
        x.qr_codes.Select(x => new QrCodeDto(x.qr_code)).ToList(),
        new FaceDto(x.face == null ? string.Empty : x.face.image_name),
        x.user_locations.Select(x => x.location.name).ToList()
      )).FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.User, username);
  }

  public async Task<Guid> GetDefaultLocationGuidAsync()
  {
    throw new NotImplementedException();
  }

  public async Task<string> GetHashByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Users
      .AsNoTracking()
      .Where(x => x.username.Equals(username))
      .Select(x => x.password)
      .FirstOrDefaultAsync() ?? throw new NotFoundException(EntityType.Operator, username);
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Users
      .AsNoTracking()
      .Where(x => x.guid == guid)
      .OrderByDescending(x => x.id)
      .Select(x => x.id)
      .FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Guid>> GetLocationGuidByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Users
      .AsNoTracking()
      .Where(x => x.username.Equals(username))
      .SelectMany(x => x.user_locations.Select(x => x.location.guid))
      .ToArrayAsync();
  }

  public async Task<Pagination<UserDto>> GetPaginationOperatorAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Users
                  .AsNoTracking()
                  .Where(x => x.is_operator)
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
              EF.Functions.ILike(x.user_code, pattern) ||
              EF.Functions.ILike(x.identification, pattern) ||
              EF.Functions.ILike(x.title.ToString(), pattern) ||
              EF.Functions.ILike(x.firstname, pattern) ||
              EF.Functions.ILike(x.middlename, pattern) ||
              EF.Functions.ILike(x.lastname, pattern) ||
              EF.Functions.ILike(x.gender.ToString(), pattern) ||
              EF.Functions.ILike(x.email, pattern) ||
              EF.Functions.ILike(x.phone, pattern) ||
              (x.company != null ? EF.Functions.ILike(x.company.name, pattern) : false) ||
              (x.department != null ? EF.Functions.ILike(x.department.name, pattern) : false) ||
              (x.position != null ? EF.Functions.ILike(x.position.name, pattern) : false) ||
              EF.Functions.ILike(x.address, pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.username.Contains(search) ||
              x.user_code.Contains(search) ||
              x.identification.Contains(search) ||
              x.title.ToString().Contains(search) ||
              x.firstname.Contains(search) ||
              x.middlename.Contains(search) ||
              x.lastname.Contains(search) ||
              x.gender.ToString().Contains(search) ||
              x.email.Contains(search) ||
              x.phone.Contains(search) ||
              (x.company != null ? x.company.name.Contains(search) : false) ||
              (x.department != null ? x.department.name.Contains(search) : false) ||
              (x.position != null ? x.position.name.Contains(search) : false) ||
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
            e.username,
            e.identification,
            e.title,
            e.firstname,
            e.middlename,
            e.lastname,
            e.gender,
            e.date_of_birth,
            e.email,
            e.phone,
            e.is_operator,
            e.is_user,
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
            e.license_plates.Select(l => new LicensePlateDto(l.license_plate)).ToList(),
            e.pins.Select(x => new PinDto(x.pin)).ToList(),
            e.qr_codes.Select(q => new QrCodeDto(q.qr_code)).ToList(),
            new FaceDto(e.face != null ? e.face.image_name : string.Empty),
            e.user_locations.Select(x => x.location.name).ToList()
          )).ToListAsync();

    return new Pagination<UserDto>(
          param.pageNumber,
          param.pageSize,
          count,
          (int)Math.Ceiling(count / (double)param.pageSize),
          res
          );
  }

  public async Task<Pagination<UserDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    var query = context.Users
                  .AsNoTracking()
                  .Where(x => x.user_locations.Any(x => x.location.guid == param.locationGuid))
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
              EF.Functions.ILike(x.user_code, pattern) ||
              EF.Functions.ILike(x.identification, pattern) ||
              EF.Functions.ILike(x.title.ToString(), pattern) ||
              EF.Functions.ILike(x.firstname, pattern) ||
              EF.Functions.ILike(x.middlename, pattern) ||
              EF.Functions.ILike(x.lastname, pattern) ||
              EF.Functions.ILike(x.gender.ToString(), pattern) ||
              EF.Functions.ILike(x.email, pattern) ||
              EF.Functions.ILike(x.phone, pattern) ||
              (x.company != null ? EF.Functions.ILike(x.company.name, pattern) : false) ||
              (x.department != null ? EF.Functions.ILike(x.department.name, pattern) : false) ||
              (x.position != null ? EF.Functions.ILike(x.position.name, pattern) : false) ||
              EF.Functions.ILike(x.address, pattern)
          );
        }
        else // SQL Server
        {
          query = query.Where(x =>
              x.username.Contains(search) ||
              x.user_code.Contains(search) ||
              x.identification.Contains(search) ||
              x.title.ToString().Contains(search) ||
              x.firstname.Contains(search) ||
              x.middlename.Contains(search) ||
              x.lastname.Contains(search) ||
              x.gender.ToString().Contains(search) ||
              x.email.Contains(search) ||
              x.phone.Contains(search) ||
              (x.company != null ? x.company.name.Contains(search) : false) ||
              (x.department != null ? x.department.name.Contains(search) : false) ||
              (x.position != null ? x.position.name.Contains(search) : false) ||
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
            e.username,
            e.identification,
            e.title,
            e.firstname,
            e.middlename,
            e.lastname,
            e.gender,
            e.date_of_birth,
            e.email,
            e.phone,
            e.is_operator,
            e.is_user,
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
            e.license_plates.Select(l => new LicensePlateDto(l.license_plate)).ToList(),
            e.pins.Select(x => new PinDto(x.pin)).ToList(),
            e.qr_codes.Select(q => new QrCodeDto(q.qr_code)).ToList(),
            new FaceDto(e.face != null ? e.face.image_name : string.Empty),
            e.user_locations.Select(x => x.location.name).ToList()
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

  public async Task<bool> IsAnyGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Users.AsNoTracking()
      .AnyAsync(x => x.guid == guid);
  }

  public async Task<bool> IsAnyUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Users
      .AsNoTracking()
      .AnyAsync(x => x.username.Equals(username));
  }

  public async Task<bool> IsDefaultAsync(Guid guid, CancellationToken ct = default)
  {
    return await context.Users
      .AsNoTracking()
      .AnyAsync(x => x.guid.Equals(guid) && x.is_default);
  }

  public async Task UpdateAsync(User entity, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Guid> GetRoleGuidByUsernameAsync(string username, CancellationToken ct = default)
  {
    return await context.Users
      .AsNoTracking()
      .Where(x => x.username.Equals(username) && x.is_operator)
      .OrderByDescending(x => x.id)
      .Select(x => x.role == null ? Guid.Empty : x.role.guid)
      .FirstOrDefaultAsync(ct);
  }

  public Task<IEnumerable<UserDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}