using Core.Application.Interfaces;
using Core.Contract.DTOs.User;
using Core.Contract.Interfaces;
using Setting.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class UserService(
  IUserRepository repo,
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
    throw new NotImplementedException();
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<UserDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<UserDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Guid> UpdateAsync(UpdateUserDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}