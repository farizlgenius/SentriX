using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Position : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public int DepartmentId { get; private set; } = default!;

      public Position(
            string Name,
            string Description,
            int DepartmentId
      )
      {
            ValidationHelper.Name(Name);
            ValidationHelper.NotMinus(DepartmentId, nameof(this.DepartmentId));
            this.Name = Name;
            this.Description = Description;
            this.DepartmentId = DepartmentId;
      }

      public Position(
            Guid Guid,
            string Name,
            string Description,
            int DepartmentId
      ) : base(Guid)
      {
            ValidationHelper.Name(Name);
            ValidationHelper.NotMinus(DepartmentId, nameof(this.DepartmentId));
            this.Name = Name;
            this.Description = Description;
            this.DepartmentId = DepartmentId;
      }
}