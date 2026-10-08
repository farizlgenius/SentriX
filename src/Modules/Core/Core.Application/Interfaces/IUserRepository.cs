using Core.Contract.DTOs.User;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IUserRepository : IBaseRepository<UserDto, User>
{
  Task<bool> IsAnyUsernameAsync(string username, CancellationToken ct = default);
  Task<bool> IsAnyIdentificationAsync(string identification, CancellationToken ct = default);
  Task<string> GetHashByUsernameAsync(string username, CancellationToken ct = default);
  Task ChangePasswordAsync(string username, string hashed, CancellationToken ct = default);
  Task<IEnumerable<Guid>> GetLocationGuidByUsernameAsync(string username, CancellationToken ct = default);
  Task<Guid> GetRoleGuidByUsernameAsync(string username, CancellationToken ct = default);
  Task<UserDto> GetByUsernameAsync(string username, CancellationToken ct = default);
  Task<Guid> GetDefaultLocationGuidAsync();
  Task UpdateImagePathAsync(Guid guid, CancellationToken ct = default);
 Task<IEnumerable<(string mac, string ip, Vendor vendor)>> GetDetailsByUserGuidAsync(Guid guid, CancellationToken ct = default);

}