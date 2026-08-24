using Core.Contract.DTOs.Operator;
using Core.Contract.DTOs.User;

namespace Core.Contract.Interfaces;

public interface IUser : IBase<UserDto, CreateUserDto, UpdateUserDto>
{
  Task<bool> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default);
}