using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Role : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public List<Module> Modules { get; private set; } = default!;
      public int LocationId { get; private set; } = default!;

      public Role(
            string Name,
            List<Module> Modules,
            int LocationId
      )
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.Modules = Modules;
            this.LocationId = LocationId;
      }
      public Role(
            Guid Guid,
            string Name,
            List<Module> Modules,
            int LocationId
      ) : base(Guid)
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.Modules = Modules;
            this.LocationId = LocationId;
      }
}