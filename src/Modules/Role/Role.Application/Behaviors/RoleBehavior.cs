using System;
using Role.Application.Interfaces;
using Role.Contract.DTOs;
using Role.Contract.Interfaces;
using Role.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Role.Application.Behaviors;

public sealed class RoleBehavior(IRoleRepository repo) : IRole
{
      public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
      {
            if (string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.NameEmpty);

            if (!await repo.IsAnyLocationIdAsync(dto.LocationId))
                  throw new BadRequestException(MessageHelper.Location.LocationInvalid);

            if (await repo.IsAnyNameWithLocationIdAsync(dto.LocationId, dto.Name))
                  throw new BadRequestException(MessageHelper.Common.DuplicatedName);

            var domain = new Roles(0, dto.Name, dto.Permissions.Select(r => new Permission(
              r.FeatureId,
              r.FeatureName,
              r.IsEnabled,
              r.IsCreated,
              r.IsUpdated,
              r.IsDeleted
            )).ToList(), dto.LocationId);

            return await repo.AddAsync(domain);

      }

      public async Task<RoleDto> DeleteByIdAsync(int id)
      {
            if (!await repo.IsAnyWithIdAsync(id))
                  throw new BadRequestException(MessageHelper.Common.RecordNotFound);

            return await repo.DeleteByIdAsync(id);
      }

      public async Task<List<RoleDto>> DeleteRangeAsync(RangeIdDto dto)
      {
            if (dto.Ids == null || dto.Ids.Count <= 0)
                  throw new BadRequestException(MessageHelper.Role.RoleInvalid);

            if (!await repo.IsAllExistByIdsAsync(dto.Ids))
                  throw new BadRequestException(MessageHelper.Role.RoleNotFound);


            return await repo.DeleteRangeAsync(dto.Ids);
      }

      public async Task<List<RoleDto>> GetByLocationIdAsync(int location)
      {
            var res = await repo.GetByLocationIdAsync(location);
            return res;

      }

      public async Task<List<FeatureDto>> GetFeaturesAsync()
      {
            var res = await repo.GetFeaturesAsync();
            return res;
      }

      public async Task<Pagination<RoleDto>> GetPagination(PaginationParams param)
      {
            var res = await repo.GetPagination(param);
            return res;
      }

      public async Task<RoleDto> UpdateAsync(UpdateRoleDto dto)
      {
            if (string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.NameEmpty);

            if (!await repo.IsAnyLocationIdAsync(dto.LocationId))
                  throw new BadRequestException(MessageHelper.Location.LocationInvalid);

            var domain = new Domain.Entities.Roles(dto.Id, dto.Name, dto.Permissions.Select(r =>
              new Permission(r.FeatureId, r.FeatureName, r.IsEnabled, r.IsCreated, r.IsUpdated, r.IsDeleted)
            ).ToList(), dto.LocationId);

            var res = await repo.UpdateAsync(domain);
            return res;

      }
}
