using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Role : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public List<Permission> Permissions {get; private set;} = default!;

      public Role(
            string Name,
            List<Permission> Permissions
      ) 
      {
            ValidationHelper.IsValidName(Name);
            this.Name = Name;
            this.Permissions = Permissions;
      }
      public Role(
            Guid Guid,
            string Name,
            List<Permission> Permissions
      ) : base(Guid)
      {
            ValidationHelper.IsValidName(Name);
            this.Name = Name;
            this.Permissions = Permissions;
      }
}