using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Department : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public Guid CompanyGuid { get; private set; } = default!;
      public Department(
            string Name,
            string Description,
            Guid CompanyGuid
      )
      {
            ValidationHelper.IsValidName(Name);
            this.Name = Name;
            this.Description = Description;
            this.CompanyGuid = CompanyGuid;
      }
      public Department(
            Guid Guid,
            string Name,
            string Description,
            Guid CompanyGuid
            ) : base(Guid)
      {
            ValidationHelper.IsValidName(Name);
            this.Name = Name;
            this.Description = Description;
            this.CompanyGuid = CompanyGuid;
      }
}