using Core.Application.Interfaces;
using Core.Contract.DTOs.Operator;
using Core.Contract.Interfaces;
using Setting.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class OperatorService(
  IOperatorRepository repo,
  IMessageBus bus
  ) : IOperator
{
  public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default)
  {
    // Check that username exits
    if (!await repo.IsAnyUsernameAsync(dto.Username))
      throw new NotFoundException(EntityType.Operator, dto.Username);

    // Get Password from username
    var hashed = await repo.GetHashByUsernameAsync(dto.Username);

    // Check old password is valid or not
    if (!PasswordHasher.VerifyPassword(dto.Old, hashed))
      throw new BadRequestException(EntityType.Operator, "Password incorrect.");

    // Validate Password
    var IsValidPassword = await bus.QueryAsync(new ValidatePasswordWithRuleQuery(dto.New));
    if (!string.IsNullOrWhiteSpace(IsValidPassword))
      throw new BadRequestException(EntityType.Operator, IsValidPassword);

    await repo.ChangePasswordAsync(dto.Username, PasswordHasher.HashPassword(dto.New), ct);

    return true;
  }

  public async Task<OperatorDto> CreateAsync(CreateOperatorDto dto, CancellationToken ct = default)
  {
    var d = new Core.Domain.Entities.Operator(
      dto.Username,
      dto.Password,
      dto.Email,
      dto.Phone,
      dto.JoinedDate,
      dto.ExpiredDate,
      dto.RoleGuid,
      dto.LocationGuids
    );

    // Check username
    if (await repo.IsAnyByNameAndLocationGuidAsync(dto.Username))
      throw new DuplicateException(EntityType.Operator, dto.Username);

    // Validate Password
    var IsValidPassword = await bus.QueryAsync(new ValidatePasswordWithRuleQuery(dto.Password));
    if (!string.IsNullOrWhiteSpace(IsValidPassword))
      throw new BadRequestException(EntityType.Operator, IsValidPassword);


    await repo.AddAsync(d, ct);

    return new OperatorDto(
      d.Guid,
      d.Username,
      d.Email,
      d.Phone,
      d.JoinedDate,
      d.ExpiredDate,
      d.RoleId,
      d.LocationIds,
      true,
      false
    );

  }

  public async Task<Guid> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Operator, guid.ToString());

    // Check is default location
    if (await repo.IsDefaultAsync(guid, ct))
      throw new DefaultRecordException(MethodType.Delete, EntityType.Operator, guid.ToString());

    // Check relate object here

    await repo.DeleteAsync(guid, ct);

    return guid;
  }

  public async Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    // Check if guids is empty 
    if (guids.Count() == 0)
      throw new NotFoundException(EntityType.Operator);

    foreach (var guid in guids)
    {
      // Check is any location with guid
      if (!await repo.IsAnyGuidAsync(guid, ct))
        throw new NotFoundException(EntityType.Operator, guid.ToString());

      // Check relate object here
    }

    await repo.DeleteRangeAsync(guids);

    return guids;
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Operator, guid.ToString());

    return await repo.DisableAsync(guid, ct);
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(guid, ct))
      throw new NotFoundException(EntityType.Operator, guid.ToString());

    return await repo.EnableAsync(guid, ct);
  }

  public async Task<OperatorDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetAsync(guid, ct);
  }

  public async Task<Pagination<OperatorDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task<OperatorDto> UpdateAsync(UpdateOperatorDto dto, CancellationToken ct = default)
  {
    // Check is any location with guid
    if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
      throw new NotFoundException(EntityType.Location, dto.Guid.ToString());

    var d = new Core.Domain.Entities.Operator(
      dto.Guid,
      dto.Username,
      dto.Email,
      dto.Phone,
      dto.JoinedDate,
      dto.ExpiredDate,
      dto.RoleGuid,
      dto.LocationGuids
    );

    await repo.UpdateAsync(d);

    return new OperatorDto(
      d.Guid,
      d.Username,
      d.Email,
      d.Phone,
      d.JoinedDate,
      d.ExpiredDate,
      d.RoleId,
      d.LocationIds,
      true,
      false
    );
  }
}