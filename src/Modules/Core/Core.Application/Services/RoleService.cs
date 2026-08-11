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
                  )).ToList(),
                  dto.LocationGuid
            );

            // Check name is duplicate 
            if (await repo.IsAnyByNameAndLocationGuidAsync(dto.Name, dto.LocationGuid))
                  throw new DuplicateException(EntityType.Role, dto.Name);

            await repo.AddAsync(d, ct);

            return new RoleDto(
                  d.Guid,
                  d.Name,
                  d.Permissions.Select(x => new PermissionDto(
                        x.Guid,
                        x.FeatureGuid,
                        x.RoleGuid,
                        x.IsEnabled,
                        x.IsCreated,
                        x.IsUpdated,
                        x.IsDeleted
                  )).ToList(),
                  d.LocationGuid,
                  true,
                  false
            );


      }

      public async Task<Guid> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Role, guid.ToString());

            // Check is default location
            if (await repo.IsDefaultAsync(guid, ct))
                  throw new DefaultRecordException(MethodType.Delete, EntityType.Role, guid.ToString());

            // Check relate object here

            if (await repo.IsAnyOperatorAsync(guid, ct))
                  throw new FoundRelateException(EntityType.Operator, guid.ToString(), EntityType.User);


            await repo.DeleteAsync(guid, ct);

            return guid;
      }

      public async Task<IEnumerable<Guid>> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            // Check if guids is empty 
            if (guids.Count() == 0)
                  throw new NotFoundException(EntityType.Role);

            foreach (var guid in guids)
            {
                  // Check is any location with guid
                  if (!await repo.IsAnyGuidAsync(guid, ct))
                        throw new NotFoundException(EntityType.Role, guid.ToString());

                  // Check relate object here
                  if (await repo.IsAnyOperatorAsync(guid, ct))
                        throw new FoundRelateException(EntityType.Operator, guid.ToString(), EntityType.User);
            }

            await repo.DeleteRangeAsync(guids);

            return guids;
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Role, guid.ToString());

            return await repo.DisableAsync(guid, ct);
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Role, guid.ToString());

            return await repo.EnableAsync(guid, ct);
      }

      public async Task<RoleDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetAsync(guid, ct);
      }

      public async Task<Pagination<RoleDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetPaginationAsync(param, ct);
      }

      public async Task<RoleDto> UpdateAsync(UpdateRoleDto dto, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
                  throw new NotFoundException(EntityType.Role, dto.Guid.ToString());

            var d = new Core.Domain.Entities.Role(
                  dto.Guid,
                  dto.Name,
                  dto.Permissions.Select(x => new Permission(
                        x.RoleGuid,
                        x.FeatureGuid,
                        x.IsEnabled,
                        x.IsCreated,
                        x.IsUpdated,
                        x.IsDeleted
                  )).ToList(),
                  dto.LocationGuid
            );

            await repo.UpdateAsync(d);

            return new RoleDto(
              d.Guid,
              d.Name,
              d.Permissions.Select(x => new PermissionDto(
                        x.Guid,
                        x.FeatureGuid,
                        x.RoleGuid,
                        x.IsEnabled,
                        x.IsCreated,
                        x.IsUpdated,
                        x.IsDeleted
                  )).ToList(),
              d.LocationGuid,
              true,
              false
            );
      }
}