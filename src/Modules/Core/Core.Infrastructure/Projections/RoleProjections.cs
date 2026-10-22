using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Role;

namespace Core.Infrastructure.Projections;

public static class RoleProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Role, RoleDto>> ToDto = 
        x => new RoleDto(
                        x.guid,
                        x.name,
                        x.module_permission.Select(x => new ModulePermissionDto(
                              x.module_id,
                              x.module.name,
                              x.is_enabled,
                              x.feature_permissions.Select(f => new FeaturePermissionDto(
                                    f.feature_id,
                                    f.feature.name,
                                    f.is_enabled,
                                    f.is_created,
                                    f.is_updated,
                                    f.is_deleted
                                    )).ToList()
                        )).ToList(),
                        x.is_active,
                        x.is_default
                  );

    public static readonly Expression<Func<Persistences.Entities.Role,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}