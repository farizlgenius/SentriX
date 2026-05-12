using System;
using Operator.Contract.DTOs;
using Operator.Domain.Entities;
using SharedKernel.Domain;

namespace Operator.Application.Interfaces;

public interface IOperatorRepository
{
      Task<string> GetPassowrdByUsernameAsync(string Username);
      Task<bool> IsOperatorExistsByUsernameAsync(string Username);
      Task<OperatorDto> GetOperatorByUsernameAsync(string Username);
      Task AddOperatorLocationsAsync(int operatorId, int locationId,CancellationToken ct = default);
      Task RemoveOperatorLocationsAsync(int locationId, CancellationToken ct);
      Task RemoveOperatorLocationByLocationIdAsync(int locationId);
      Task<Pagination<OperatorDto>> GetPagination(PaginationParams param,CancellationToken ct = default);
      Task<bool> IsAnyWithLocationIdAsync(int LocationId);
      Task<bool> IsAnyUsernameAsync(string Username);
      Task<OperatorDto> AddAsync(Operators domain);
      Task<OperatorDto> UpdateAsync(Operators domain);
      Task<bool> IsAnyByIdAsync(int id);
      Task<OperatorDto> DeleteByIdAsync(int id);
      Task<bool> IsLocationIdsValidAsync(List<int> LocationIds);
      Task<bool> IsValidRoleIdAsync(int RoleId);

      // New 
      Task<List<int>> GetLocationIdsByUsernameAsync(string username, CancellationToken ct);
      Task<int> GetLocationIdByUsernameAsync(string username);
      Task<PasswordRuleDto> GetPassowrdRuleAsync();
      Task<PasswordRuleDto> CreatePasswordRuleAsync(PasswordRule rule);

}
