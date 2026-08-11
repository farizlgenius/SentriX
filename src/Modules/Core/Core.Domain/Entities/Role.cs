using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Role : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public List<Permission> Permissions { get; private set; } = default!;
      public Guid LocationGuid { get; private set; } = default!;

      public Role(
            string Name,
            List<Permission> Permissions,
            Guid LocationGuid
      )
      {
            ValidationHelper.IsValidName(Name);
            this.Name = Name;
            this.Permissions = Permissions;
            this.LocationGuid = LocationGuid;
      }
      public Role(
            Guid Guid,
            string Name,
            List<Permission> Permissions,
            Guid LocationGuid
      ) : base(Guid)
      {
            ValidationHelper.IsValidName(Name);
            this.Name = Name;
            this.Permissions = Permissions;
            this.LocationGuid = LocationGuid;
      }
}