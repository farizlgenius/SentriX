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
    throw new NotImplementedException();
  }

  public async Task<int> GetIdByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Guid>> GetLocationGuidByUsernameAsync(string username, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<UserDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
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
}