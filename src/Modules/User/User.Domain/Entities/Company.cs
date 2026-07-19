using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Company : BaseDomainEntity
{
      public string Name {get; private set;} = string.Empty;      
      public string Description {get; private set;} = string.Empty;
      public string Address {get; private set;} = string.Empty;
      public List<Department> Departments {get; private set;} = new List<Department>();
      public List<Users> Users {get; private set;} = new List<Users>();

      public Company(Guid guid, string name, string address, string description, int locationId, bool isActive,bool isDefault) : base(guid, 0, locationId, isActive,isDefault)
      {
            ValidationHelper.IsNullOrEmpty(name, nameof(name));
            ValidationHelper.ValidateNotMinus(locationId, nameof(locationId));
            Name = name;
            Address = address;
            Description = description;
            LocationId = locationId;
      }

}
