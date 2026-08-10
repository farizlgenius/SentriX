using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Role : BaseDomain
{
      public string Name {get; private set;} = string.Empty;

      public Role(
            string Name
      ) 
      {
            ValidationHelper.IsValidName(Name);
            this.Name = Name;
      }
      public Role(
            Guid Guid,
            string Name
      ) : base(Guid)
      {
            ValidationHelper.IsValidName(Name);
            this.Name = Name;
      }
}