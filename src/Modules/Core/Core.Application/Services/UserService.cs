using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.User;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Domain.Entities;
using Setting.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using Storage.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class UserService(
  IUserRepository repo,
  IRoleRepository role,
  ICompanyRepository company,
  IGroupRepository group,
  IAdapterFactory adapter,
  IComponentMappingRepository com,
  IStorage file,
  IMessageBus bus
  ) : IUser
{
  public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default)
  {
    // Check that username exits
    if (!await repo.IsAnyUsernameAsync(dto.Username))
      throw new NotFoundException(EntityType.User.ToString(), dto.Username);

    // Get Password from username
    var hashed = await repo.GetHashByUsernameAsync(dto.Username);

    // Check old password is valid or not
    if (!PasswordHasher.VerifyPassword(dto.Old, hashed))
      throw new BadRequestException(EntityType.User.ToString(), "Password incorrect.");

    // Validate Password
    var IsValidPassword = await bus.QueryAsync(new ValidatePasswordWithRuleQuery(dto.New));
    if (!string.IsNullOrWhiteSpace(IsValidPassword))
      throw new BadRequestException(EntityType.User.ToString(), IsValidPassword);

    await repo.ChangePasswordAsync(dto.Username, PasswordHasher.HashPassword(dto.New), ct);

    return true;
  }

  public async Task<Guid> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
  {

    //var roleId = dto.RoleGuid == Guid.Empty ? 0 : await bus.QueryAsync(new RoleIdByGuidQuery(dto.RoleGuid));
    var roleId = dto.RoleGuid == null ? 0 : await role.GetIdByGuidAsync(dto.RoleGuid ?? Guid.Empty,ct);
    //var companyId = dto.CompanyGuid == Guid.Empty ? 0 : await bus.QueryAsync(new CompanyIdByGuidQuery(dto.CompanyGuid));
    var companyId = dto.CompanyGuid == null ? 0 : await company.GetIdByGuidAsync(dto.CompanyGuid ?? Guid.Empty,ct);
    var departmentId = dto.DepartmentGuid == null ? 0 : await bus.QueryAsync(new DepartmentIdByGuidQuery(dto.DepartmentGuid ?? Guid.Empty), ct);
    var positionId = dto.PositionGuid == null ? 0 : await bus.QueryAsync(new PositionIdByGuidQuery(dto.PositionGuid ?? Guid.Empty), ct);
    var locationIds = dto.Locations.Count == 0 ? [] : await bus.QueryAsync(new LocationIdsByGuidsQuery(dto.Locations), ct);
    var groupIds = dto.Groups.Count == 0 ? [] : await bus.QueryAsync(new GroupIdsByGuidsQuery(dto.Groups), ct);
    var details = dto.Groups.Count == 0 ? [] : await group.GetDetailsByGroupGuidsAsync(dto.Groups,ct);

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
      roleId,
      companyId,
      departmentId,
      positionId,
      dto.Metadata,
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
      throw new DuplicateException(EntityType.User.ToString(), dto.Username);

    if (await repo.IsAnyIdentificationAsync(dto.Identification))
      throw new DuplicateException(EntityType.User.ToString(), dto.Username);


    // 1.Get mac or device list that this user in by group
    var deviceDetail = await group.GetDetailsByGroupGuidsAsync(dto.Groups,ct);

    // 2.Send credential to each device
    foreach (var dev in deviceDetail)
    {
      // Send card detail
      int i = 0;
      foreach (var c in dto.Cards)
      {
        var deviceExternalId = await com.GetExternalIdByMacAndEntityAsync(dev.mac, EntityType.Device, ct);
        var groupExternaIds = await com.GetExternalIdsByGuidsAndEntityAsync(dto.Groups,EntityType.Group,ct);
        await adapter.GetAdapter(dev.vendor).User.AddUserAsync(
          dev.mac,
          dev.ip,
          (short)deviceExternalId,
          c.CardNumber,
          i == 0 && dto.Pin != null ? dto.Pin.Pin : string.Empty,
          groupExternaIds.Select(x => (short)x).ToList(),
          dto.JoinedDate,
          dto.ExpiredDate,
          dto.Metadata,
          ct
        );
        i++;
      }

    }

    await repo.AddAsync(d, ct);

    return d.Guid;

  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.User.ToString(), guid.ToString());

    // Send command to delete user from device
    var user = await repo.GetAsync(guid,ct);
    var deviceDetail = await repo.GetDetailsByUserGuidAsync(guid,ct);

    foreach (var dev in deviceDetail)
    {
      var deviceId = await com.GetExternalIdByMacAndEntityAsync(dev.mac,EntityType.Device,ct);
      foreach (var c in user.Cards)
      {
        await adapter.GetAdapter(dev.vendor).User.DeleteUserAsync(
        dev.mac,
        dev.ip,
        (short)deviceId,
        c.CardNumber,
        ct);
      }

    }

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
      throw new NotFoundException(EntityType.User.ToString(), guid.ToString());

    // Send Command to delete user from device

    await repo.DisableAsync(guid, ct);

    return true;

  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.User.ToString(), guid.ToString());

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
    return await repo.GetByLocationAsync(guid, ct);
  }

  public async Task<Stream?> GetImageByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      return null;
      //throw new NotFoundException(EntityType.User.ToString(), guid.ToString());

    return await file.ReadUserAsync(guid.ToString());
  }


  public async Task<Pagination<UserDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateUserDto dto, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(dto.Guid))
      throw new NotFoundException(EntityType.User.ToString(), dto.Guid.ToString());

    var roleId = dto.RoleGuid == null ? 0 :  await bus.QueryAsync(new RoleIdByGuidQuery(dto.RoleGuid ?? Guid.Empty));
    var companyId = dto.CompanyGuid == null ? 0 : await bus.QueryAsync(new CompanyIdByGuidQuery(dto.CompanyGuid ?? Guid.Empty));
    var departmentId = dto.DepartmentGuid == null ? 0 : await bus.QueryAsync(new DepartmentIdByGuidQuery(dto.DepartmentGuid ?? Guid.Empty));
    var positionId = dto.PositionGuid == null ? 0 : await bus.QueryAsync(new PositionIdByGuidQuery(dto.PositionGuid ?? Guid.Empty));
    var locationIds = await bus.QueryAsync(new LocationIdsByGuidsQuery(dto.Locations));
    var groupIds = dto.Groups.Count == 0 ? [] : await bus.QueryAsync(new GroupIdsByGuidsQuery(dto.Groups));

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
      roleId,
      companyId,
      departmentId,
      positionId,
      dto.Metadata,
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

      public Task UploadAsync(CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> UploadImageAsync(Guid guid, Stream stream, CancellationToken ct = default)
  {
    if (!await repo.IsAnyGuidAsync(guid))
      throw new NotFoundException(EntityType.User.ToString(), guid.ToString());


    var path = await file.SaveUserAsync(stream, guid.ToString());

    await repo.UpdateImagePathAsync(guid, ct);

    return true;
  }
}