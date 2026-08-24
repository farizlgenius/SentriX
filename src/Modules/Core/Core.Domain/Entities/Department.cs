using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Department : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public int CompanyId { get; private set; } = default!;
      public Department(
            string Name,
            string Description,
            int CompanyId
      )
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.Description = Description;
            this.CompanyId = CompanyId;
      }
      public Department(
            Guid Guid,
            string Name,
            string Description,
            int CompanyId
            ) : base(Guid)
      {
            ValidationHelper.Name(Name);
            this.Name = Name;
            this.Description = Description;
            this.CompanyId = CompanyId;
      }
}