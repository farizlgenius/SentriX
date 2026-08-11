using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Position : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public string Description {get; private set;} =string.Empty;
      public Guid DepartmentGuid {get; private set;} = default!;

      public Position(
            string Name,
            string Description,
            Guid DepartmentGuid
      )
      {
            ValidationHelper.IsValidName(Name);
            ValidationHelper.ValidateGuid(DepartmentGuid,nameof(this.DepartmentGuid));
            this.Name = Name;
            this.Description = Description;
            this.DepartmentGuid = DepartmentGuid;
      }

      public Position(
            Guid Guid,
            string Name,
            string Description,
            Guid DepartmentGuid
      ) : base(Guid)
      {
            ValidationHelper.IsValidName(Name);
            ValidationHelper.ValidateGuid(DepartmentGuid,nameof(this.DepartmentGuid));
            this.Name = Name;
            this.Description = Description;
            this.DepartmentGuid = DepartmentGuid;
      }
}