using System;
using Role.Contract.DTOs;
using Role.Domain.Entities;
using SharedKernel.Domain;

namespace Role.Application.Interfaces;

public interface IRoleRepository
{
      Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int RoleId, CancellationToken ct = default);
      Task<bool> IsAnyLocationIdAsync(int LocationId);
      Task<bool> IsAnyNameWithLocationIdAsync(int LocationId, string Name);
      Task<Pagination<RoleDto>> GetPagination(PaginationParams param,CancellationToken ct = default);
      Task<RoleDto> AddAsync(Roles domain);
      Task<bool> IsAnyWithIdAsync(int id);
      Task<RoleDto> DeleteByIdAsync(int id);
      Task<RoleDto> UpdateAsync(Roles domain);
      Task<List<FeatureDto>> GetFeaturesAsync();
      Task<bool> IsAllExistByIdsAsync(List<int> ids);
      Task<List<RoleDto>> DeleteRangeAsync(List<int> ids);
      Task<List<RoleDto>> GetByLocationIdAsync(int id);
      Task<bool> IsValidRoleIdAsync(int RoleId,CancellationToken ct = default);

}
