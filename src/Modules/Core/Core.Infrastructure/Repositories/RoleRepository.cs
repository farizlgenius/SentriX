using Core.Application.Interfaces;
using Core.Contract.DTOs.Role;
using Core.Domain.Entities;
using Core.Infrastructure.Persistences;
using Core.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Infrastructure.Repositories;

public sealed class RoleRepository(CoreDbContext context) : BaseRepository<Persistences.Entities.Role,Domain.Entities.Role,RoleDto>(
      context,
      d => new Persistences.Entities.Role(d),
      (en,dom) => {},
      RoleProjections.ToDto,
      RoleProjections.ToCommand
),IRoleRepository
{
    

      public async Task<IEnumerable<RoleDto>> GetAsync(CancellationToken ct = default)
      {
            return await Context.Roles
                  .AsNoTracking()
                  .Select(e => new RoleDto(
                        e.guid,
                        e.name,
                        new List<ModulePermissionDto>(),
                        e.is_active,
                        e.is_default
                  )).ToArrayAsync(cancellationToken: ct);
      }


    

      public async Task<Pagination<RoleDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await base.GetPaginationAsync(
                  param,
                  param.locationGuid != new Guid("3a9c9947-d5ca-4bb2-b525-0499a340f1d6") ? x => x.location.guid == param.locationGuid : null,
                  [x => x.name],
                  ct
            );
      }

      public async Task<IEnumerable<ModulePermissionDto>> GetPermissionByRoleIdAsync(int id, CancellationToken ct = default)
      {
            return await Context.ModulePermissions
                  .AsNoTracking()
                  .Where(x => x.role_id == id)
                  .Select(x => new ModulePermissionDto(
                        x.module_id,
                        x.module.name,
                        x.is_enabled,
                        x.feature_permissions.Select(x => new FeaturePermissionDto(
                              x.feature_id,
                              x.feature.name,
                              x.is_enabled,
                              x.is_created,
                              x.is_updated,
                              x.is_deleted
                        )).ToList()
                  )).ToArrayAsync(cancellationToken: ct);
      }

    
      public async Task<bool> IsAnyByNameAndLocationIdAsync(string name, int locationId = default, CancellationToken ct = default)
      {
            return await Context.Roles
                  .AsNoTracking()
                  .AnyAsync(x => x.name.Equals(name), cancellationToken: ct);
      }

     

      public async Task<bool> IsAnyNameAsync(string Name, Guid locationGuid = default, CancellationToken ct = default)
      {
            return await Context.Roles
                  .AsNoTracking()
                  .AnyAsync(x => x.name.Equals(Name), cancellationToken: ct);
      }

      public Task<bool> IsAnyRelatedEntitiesAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyUserByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await Context.Roles
                  .AsNoTracking()
                  .AnyAsync(x => x.guid == guid && x.users.Any(), cancellationToken: ct);

      }



      public override async Task UpdateAsync(Role entity, CancellationToken ct = default)
      {
            
            try
            {
                  await Context.Database.BeginTransactionAsync(ct);

                  var en = await Context.Roles
                  .Include(x => x.module_permission)
                  .ThenInclude(x => x.feature_permissions)
                  .Where(x => x.guid == entity.Guid)
                  .FirstOrDefaultAsync(cancellationToken: ct) ?? throw new NotFoundException(EntityType.ROLE.ToString(), entity.Guid.ToString());

                  // Delete old permission
                  Context.ModulePermissions.RemoveRange(en.module_permission);

                  en.name = entity.Name;
                  en.module_permission = entity.ModulePermissions.Select(x => new Core.Infrastructure.Persistences.Entities.ModulePermission(
                        x
                  )).ToList();

                  Context.Roles.Update(en);

                  await Context.SaveChangesAsync(ct);

                  await Context.Database.CommitTransactionAsync(ct);
            }
            catch
            {
                  await Context.Database.RollbackTransactionAsync(ct);
                  throw;
            }

      }
}