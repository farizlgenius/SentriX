using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Role : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public List<Permission> Permissions { get; private set; } = default!;
      public int LocationId { get; private set; } = default!;

      public Role(
            string Name,
            List<Permission> Permissions,
            int LocationId
      )
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.Permissions = Permissions;
            this.LocationId = LocationId;
      }
      public Role(
            Guid Guid,
            string Name,
            List<Permission> Permissions,
            int LocationId
      ) : base(Guid)
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.Permissions = Permissions;
            this.LocationId = LocationId;
      }
}