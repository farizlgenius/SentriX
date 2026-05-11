using System;
using Role.Contract.DTOs;
using Role.Contract.Interfaces;
using SharedKernel.Domain;

namespace Role.Application.Behaviors;

public sealed class RoleBehavior : IRole
{
      public Task<RoleDto> CreateAsync(CreateRoleDto dto)
      {
            throw new NotImplementedException();
      }

      public Task<RoleDto> DeleteByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public Task<List<RoleDto>> DeleteRangeAsync(RangeIdDto ids)
      {
            throw new NotImplementedException();
      }

      public Task<List<RoleDto>> GetByLocationIdAsync(int location)
      {
            throw new NotImplementedException();
      }

      public Task<List<FeatureDto>> GetFeaturesAsync()
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<RoleDto>> GetPaginationWithLocationIdAsync(int location, int Page, int PageSize)
      {
            throw new NotImplementedException();
      }

      public Task<RoleDto> UpdateAsync(UpdateRoleDto dto)
      {
            throw new NotImplementedException();
      }
}
