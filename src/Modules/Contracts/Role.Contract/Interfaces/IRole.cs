using System;
using Role.Contract.DTOs;
using SharedKernel.Domain;

namespace Role.Contract.Interfaces;

public interface IRole
{
      Task<Pagination<RoleDto>> GetPaginationWithLocationIdAsync(int location, int Page, int PageSize);
      Task<RoleDto> CreateAsync(CreateRoleDto dto);
      Task<RoleDto> DeleteByIdAsync(int id);
      Task<RoleDto> UpdateAsync(UpdateRoleDto dto);
      Task<List<FeatureDto>> GetFeaturesAsync();
      Task<List<RoleDto>> DeleteRangeAsync(RangeIdDto ids);
      Task<List<RoleDto>> GetByLocationIdAsync(int location);
}
