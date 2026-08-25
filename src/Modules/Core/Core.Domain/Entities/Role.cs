using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Role : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public List<Module> Modules { get; private set; } = default!;
      public List<int> LocationIds { get; private set; } = default!;

      public Role(
            string Name,
            List<Module> Modules,
            List<int>  LocationIds
      )
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.Modules = Modules;
            this.LocationIds = LocationIds;
      }
      public Role(
            Guid Guid,
            string Name,
            List<Module> Modules,
            List<int> LocationIds
      ) : base(Guid)
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.Modules = Modules;
            this.LocationIds = LocationIds;
      }
}