namespace Core.Domain.Entities;

public sealed class Company : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public string Description {get; private set;} = string.Empty;
      public string Address {get; private set;} = string.Empty;

      public Company(
            string name,
            string description,
            string address
      )
      {
            Name = name;
            Description = description;
            Address = address;
      }
}