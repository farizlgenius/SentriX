using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Company : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public string Address { get; private set; } = string.Empty;
      public Guid LocationGuid { get; private set; } = default!;

      public Company(
            string name,
            string description,
            string address,
            Guid locationGuid
      )
      {
            ValidationHelper.IsValidName(name);
            Name = name;
            Description = description;
            Address = address;
            LocationGuid = locationGuid;
      }

      public Company(
            Guid guid,
           string name,
           string description,
           string address,
           Guid locationGuid
     ) : base(guid)
      {
            ValidationHelper.IsValidName(name);
            ValidationHelper.ValidateGuid(guid, nameof(this.Guid));
            Name = name;
            Description = description;
            Address = address;
            LocationGuid = locationGuid;
      }
}