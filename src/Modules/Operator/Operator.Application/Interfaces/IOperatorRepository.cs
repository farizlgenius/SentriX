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
      Task AddOperatorLocationsAsync(int operatorId, int locationId,CancellationToken ct);
      Task RemoveOperatorLocationsAsync(int locationId, CancellationToken ct);
      Task RemoveOperatorLocationByLocationIdAsync(int locationId);
      Task<Pagination<OperatorDto>> GetPaginationWithLocationIdAsync(int LocationId, int Page, int PageSize,string Search);
      Task<bool> IsAnyWithLocationIdAsync(int LocationId);
      Task<bool> IsAnyUsernameAsync(string Username);
      Task<OperatorDto> AddAsync(Operators domain);
      Task<OperatorDto> UpdateAsync(Operators domain);
      Task<bool> IsAnyByIdAsync(int id);
      Task<OperatorDto> DeleteByIdAsync(int id);
      Task<bool> IsValidCompanyAsync(int CompanyId);
      Task<bool> IsValidDepartmentAsync(int DepartmentId);
      Task<bool> IsValidPositionAsync(int PositionId);
      Task<bool> IsExceptLocationIdsAsync(List<int> LocationIds);
      Task<bool> IsValidRoleIdAsync(int RoleId);

      // New 
      Task<List<int>> GetLocationIdsByUsernameAsync(string username, CancellationToken ct);

}
