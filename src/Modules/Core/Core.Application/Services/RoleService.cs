using Core.Application.Interfaces;
using Core.Contract.DTOs.Role;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class RoleService(
      IRoleRepository repo,
      IFeatureRepository feature,
      IModuleRepository module
      ) : IRole
{
      public async Task<Guid> CreateAsync(CreateRoleDto dto, CancellationToken ct = default)
      {
            // Check name is duplicate 
            if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name))
                  throw new DuplicateException(EntityType.Role, dto.Name);

            var moduleGuids = dto.ModulePermissions
            .Select(x => x.Guid)
            .Distinct()
            .ToArray();

            var moduleMap = await module.GetMapGuidAndMapByGuidsAsync(moduleGuids, ct);

            var allGuids = dto.ModulePermissions
                  .SelectMany(x => x.FeaturePermission.Select(f => f.Guid))
                  .Distinct()
                  .ToList();

            var featureMap = await feature.GetMapIdGuidByGuidsAsync(allGuids, ct);

            var d = new Core.Domain.Entities.Role(
                  dto.Name,
                  dto.ModulePermissions.Select(x => new ModulePermission(
                        x.IsEnabled,
                        moduleMap[x.Guid].Item1,
                        x.FeaturePermission.Select(s => new FeaturePermission(
                              featureMap[s.Guid].Item1,
                              s.IsEnabled,
                              s.IsCreated,
                              s.IsUpdated,
                              s.IsDeleted
                        )).ToList()
                  )).ToList()
            );

            await repo.AddAsync(d, ct);

            return d.Guid;

      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.Role, guid.ToString());

            // Check is default location
            if (await repo.IsDefaultAsync(guid, ct))
                  throw new DefaultRecordException(MethodType.Delete, EntityType.Role, guid.ToString());

            // Check relate object here

            if (await repo.IsAnyUserByGuidAsync(guid, ct))
                  throw new FoundRelateException(EntityType.Operator, guid.ToString(), EntityType.User);


            await repo.DeleteAsync(guid, ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
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
                  if (await repo.IsAnyUserByGuidAsync(guid, ct))
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

      public async Task<Guid> UpdateAsync(UpdateRoleDto dto, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
                  throw new NotFoundException(EntityType.Role, dto.Guid.ToString());

            var moduleAllGuids = dto.ModulePermissions.Select(x => x.Guid).ToArray();

            var moduleMap = await module.GetMapGuidAndMapByGuidsAsync(moduleAllGuids, ct);

            var allGuids = dto.ModulePermissions
                  .SelectMany(x => x.FeaturePermission.Select(f => f.Guid))
                  .Distinct()
                  .ToList();

            var featureMap = await feature.GetMapIdGuidByGuidsAsync(allGuids, ct);

            var d = new Core.Domain.Entities.Role(
                  dto.Name,
                  dto.ModulePermissions.Select(x => new ModulePermission(
                        x.IsEnabled,
                        moduleMap[x.Guid].Item1,
                        x.FeaturePermission.Select(s => new FeaturePermission(
                              featureMap[s.Guid].Item1,
                              s.IsEnabled,
                              s.IsCreated,
                              s.IsUpdated,
                              s.IsDeleted
                        )).ToList()
                  )).ToList()
            );

            await repo.UpdateAsync(d);

            return d.Guid;
      }
}