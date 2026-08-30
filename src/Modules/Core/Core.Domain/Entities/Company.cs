using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Company : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public string Address { get; private set; } = string.Empty;
      public Company(
            string name,
            string description,
            string address
      )
      {
            ValidationHelper.Name(name);
            Name = name;
            Description = description;
            Address = address;

      }

      public Company(
            Guid guid,
           string name,
           string description,
           string address

     ) : base(guid)
      {
            ValidationHelper.Name(name);
            ValidationHelper.GuidEmpty(guid, nameof(this.Guid));
            Name = name;
            Description = description;
            Address = address;

      }
}