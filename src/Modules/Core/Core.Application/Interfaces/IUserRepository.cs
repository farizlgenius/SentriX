using Core.Contract.DTOs.User;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface IUserRepository : IBaseRepository<UserDto, User>
{
  Task<bool> IsAnyUsernameAsync(string username, CancellationToken ct = default);
  Task<string> GetHashByUsernameAsync(string username, CancellationToken ct = default);
  Task ChangePasswordAsync(string username, string hashed, CancellationToken ct = default);
  Task<IEnumerable<Guid>> GetLocationGuidByUsernameAsync(string username, CancellationToken ct = default);
  Task<Guid> GetRoleGuidByUsernameAsync(string username, CancellationToken ct = default);
  Task<UserDto> GetByUsernameAsync(string username, CancellationToken ct = default);
  Task<Guid> GetDefaultLocationGuidAsync();
  Task<Pagination<UserDto>> GetPaginationOperatorAsync(PaginationParams param, CancellationToken ct = default);


}