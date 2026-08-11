using Core.Application.Interfaces;
using Core.Contract.DTOs.Role;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class RoleService(IRoleRepository repo) : IRole
{
      public async Task<RoleDto> CreateAsync(CreateRoleDto dto, CancellationToken ct = default)
      {
            var d = new Core.Domain.Entities.Role(
                  dto.Name,
                  dto.Permissions.Select(x => new Permission(
                        x.RoleGuid,
                        x.FeatureGuid,
                        x.IsEnabled,
                        x.IsCreated,
                        x.IsUpdated,
                        x.IsDeleted
                  )).ToList()
            );

            // Check name is duplicate 
            if (await repo.IsAnyByNameAsync(dto.Name))
                  throw new DuplicateException(EntityType.Role, dto.Name);
      }

      public Task<Guid> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<RoleDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<Pagination<RoleDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<RoleDto> UpdateAsync(UpdateRoleDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}