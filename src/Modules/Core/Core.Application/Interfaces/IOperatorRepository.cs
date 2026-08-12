using Core.Contract.DTOs.Operator;
using Core.Contract.DTOs.Role;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IOperatorRepository : IBaseRepository<OperatorDto, Operator>
{
  Task<bool> IsAnyUsernameAsync(string username, CancellationToken ct = default);
  Task<string> GetHashByUsernameAsync(string username, CancellationToken ct = default);
  Task ChangePasswordAsync(string username, string hashed, CancellationToken ct = default);
  Task<IEnumerable<Guid>> GetLocationGuidByUsernameAsync(string username, CancellationToken ct = default);
  Task<OperatorDto> GetByUsernameAsync(string username, CancellationToken ct = default);
  Task<Guid> GetDefaultLocationGuidAsync();

}