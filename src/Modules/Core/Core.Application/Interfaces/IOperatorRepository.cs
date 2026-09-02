using Core.Contract.DTOs.Operator;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface IOperatorRepository : IBaseRepository<OperatorDto, Operator>
{
      Task<string> GetPassowrdByUsernameAsync(string username, CancellationToken ct = default);
      Task<bool> IsOperatorExistsByUsernameAsync(string username, CancellationToken ct = default);
      Task<OperatorDto> GetOperatorByUsernameAsync(string username, CancellationToken ct = default);
      Task AddOperatorLocationsAsync(int operatorId, int locationId, CancellationToken ct = default);
      Task RemoveOperatorLocationsAsync(int locationId, CancellationToken ct = default);
      Task RemoveOperatorLocationByLocationIdAsync(int locationId, CancellationToken ct = default);

      Task<bool> IsAnyUsernameAsync(string username, CancellationToken ct = default);
      Task<bool> IsLocationIdsValidAsync(List<int> LocationIds, CancellationToken ct = default);
      Task<bool> IsAnyEmailAsync(string email, CancellationToken ct = default);

      // New 
      Task<IEnumerable<Guid>> GetLocationGuidsByUsernameAsync(string username, CancellationToken ct = default);
      Task<Guid> GetRoleGuidByUsernameAsync(string username, CancellationToken ct = default);
}