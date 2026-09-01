using Core.Contract.DTOs.User;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IUser : IBase<UserDto, CreateUserDto, UpdateUserDto>
{
  Task<bool> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default);
  Task<Pagination<UserDto>> GetOnlyUserAsync(PaginationParams param, CancellationToken ct = default);
  Task<Pagination<UserDto>> GetOnlyOperatorAsync(PaginationParams param, CancellationToken ct = default);
  Task<Stream> GetImageByGuidAsync(Guid guid, CancellationToken ct = default);
  Task<bool> UploadImageAsync(Guid guid, Stream stream, CancellationToken ct = default);
}