using Core.Application.Interfaces;
using Core.Contract.DTOs.User;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
using Setting.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using Storage.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class UserService(
  IUserRepository repo,
  IStorage file,
  IMessageBus bus
  ) : IUser
{
  public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default)
  {
    // Check that username exits
    if (!await repo.IsAnyUsernameAsync(dto.Username))
      throw new NotFoundException(EntityType.User, dto.Username);

    // Get Password from username
    var hashed = await repo.GetHashByUsernameAsync(dto.Username);

    // Check old password is valid or not
    if (!PasswordHasher.VerifyPassword(dto.Old, hashed))
      throw new BadRequestException(EntityType.User, "Password incorrect.");

    // Validate Password
    var IsValidPassword = await bus.QueryAsync(new ValidatePasswordWithRuleQuery(dto.New));
    if (!string.IsNullOrWhiteSpace(IsValidPassword))
      throw new BadRequestException(EntityType.User, IsValidPassword);

    await repo.ChangePasswordAsync(dto.Username, PasswordHasher.HashPassword(dto.New), ct);

    return true;
  }

  public async Task<Guid> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
  {

    var roleId = dto.RoleGuid == Guid.Empty ? 0 : await bus.QueryAsync(new RoleIdByGuidQuery(dto.RoleGuid));
    var companyId = dto.CompanyGuid == Guid.Empty ? 0 : await bus.QueryAsync(new CompanyIdByGuidQuery(dto.CompanyGuid));
    var departmentId = dto.DepartmentGuid == Guid.Empty ? 0 : await bus.QueryAsync(new DepartmentIdByGuidQuery(dto.DepartmentGuid));
    var positionId = dto.PositionGuid == Guid.Empty ? 0 : await bus.QueryAsync(new PositionIdByGuidQuery(dto.PositionGuid));
    var locationIds = dto.Locations.Count() == 0 ? new List<int>() : await bus.QueryAsync(new LocationIdsByGuidsQuery(dto.Locations));
    var groupIds = dto.Groups.Count() == 0 ? new List<int>() : await bus.QueryAsync(new GroupIdsByGuidsQuery(dto.Groups));

    var d = new User(
      dto.UserCode,
      dto.Username,
      dto.Password,
      dto.Identification,
      dto.Title,
      dto.Firstname,
      dto.Middlename,
      dto.Lastname,
      dto.Gender,
      dto.DateOfBirth,
      dto.Email,
      dto.Phone,
      dto.Address,
      dto.JoinedDate,
      dto.ExpiredDate,
      dto.Additionals,
      locationIds.ToList(),
      groupIds.ToList(),
      dto.IsOperator,
      dto.IsUser,
      roleId,
      companyId,
      departmentId,
      positionId,
      dto.Cards.Select(x => new Card(
        x.Bits,
        x.Fac,
        x.CardNumber
        )).ToList(),
      dto.LicensePlate is null || string.IsNullOrWhiteSpace(dto.LicensePlate.LicensePlate) ? null : new LicensePlate(dto.LicensePlate.LicensePlate),
      dto.Pin is null || string.IsNullOrWhiteSpace(dto.Pin.Pin) ? null : new Pin(dto.Pin.Pin),
      dto.QrCode is null || string.IsNullOrWhiteSpace(dto.QrCode.QrCode) ? null : new QrCode(dto.QrCode.QrCode)
    );
    // Check that if username and identification is already exists
    if (await repo.IsAnyUsernameAsync(dto.Username))
      throw new DuplicateException(EntityType.User, dto.Username);

    if (await repo.IsAnyIdentificationAsync(dto.Identification))
      throw new DuplicateException(EntityType.User, dto.Username);

    // Send Command to Device 

    await repo.AddAsync(d, ct);

    return d.Guid;

  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.User, guid.ToString());

    // Send command to delete user from device

    await repo.DeleteAsync(guid, ct);

    return true;
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.User, guid.ToString());

    // Send Command to delete user from device

    await repo.DisableAsync(guid, ct);

    return true;

  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.User, guid.ToString());

    // Send Command to add user to device

    await repo.EnableAsync(guid, ct);

    return true;
  }

  public async Task<UserDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<IEnumerable<UserDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(guid));
    return await repo.GetByLocationAsync(locationId, ct);
  }

  public async Task<Stream> GetImageByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.User, guid.ToString());

    return await file.ReadUserAsync(guid.ToString());
  }

  public async Task<Pagination<UserDto>> GetOnlyOperatorAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationOperatorAsync(param, ct);
  }

  public async Task<Pagination<UserDto>> GetOnlyUserAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationUserAsync(param, ct);
  }

  public async Task<Pagination<UserDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateUserDto dto, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(dto.Guid))
      throw new NotFoundException(EntityType.User, dto.Guid.ToString());

    var roleId = await bus.QueryAsync(new RoleIdByGuidQuery(dto.RoleGuid));
    var companyId = await bus.QueryAsync(new CompanyIdByGuidQuery(dto.CompanyGuid));
    var departmentId = await bus.QueryAsync(new DepartmentIdByGuidQuery(dto.DepartmentGuid));
    var positionId = await bus.QueryAsync(new PositionIdByGuidQuery(dto.PositionGuid));
    var locationIds = await bus.QueryAsync(new LocationIdsByGuidsQuery(dto.Locations));
    var groupIds = await bus.QueryAsync(new GroupIdsByGuidsQuery(dto.Groups));

    var d = new User(
      dto.UserCode,
      dto.Username,
      dto.Identification,
      string.Empty,
      dto.Title,
      dto.Firstname,
      dto.Middlename,
      dto.Lastname,
      dto.Gender,
      dto.DateOfBirth,
      dto.Email,
      dto.Phone,
      dto.Address,
      dto.JoinedDate,
      dto.ExpiredDate,
      dto.Additionals,
      locationIds.ToList(),
      groupIds.ToList(),
      dto.IsOperator,
      dto.IsUser,
      roleId,
      companyId,
      departmentId,
      positionId,
      dto.Cards.Select(x => new Card(
        x.Bits,
        x.Fac,
        x.CardNumber
        )).ToList(),
      dto.LicensePlate is null ? null : new LicensePlate(dto.LicensePlate.LicensePlate),
      dto.Pin is null ? null : new Pin(dto.Pin.Pin),
      dto.QrCode is null ? null : new QrCode(dto.QrCode.QrCode)
    );

    // Send command to update user to device

    await repo.UpdateAsync(d, ct);

    return d.Guid;


  }

  public async Task<bool> UploadImageAsync(Guid guid, Stream stream, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.User, guid.ToString());


    var path = await file.SaveUserAsync(stream, guid.ToString());

    await repo.UpdateImagePathAsync(guid, ct);

    return true;
  }
}