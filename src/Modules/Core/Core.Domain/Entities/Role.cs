using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Role : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public List<ModulePermission> ModulePermissions { get; private set; } = default!;

      public Role(
            string Name,
            List<ModulePermission> ModulePermissions
      )
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.ModulePermissions = ModulePermissions;
      }
      public Role(
            Guid Guid,
            string Name,
            List<ModulePermission> ModulePermissions
      ) : base(Guid)
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.ModulePermissions = ModulePermissions;

      }
}