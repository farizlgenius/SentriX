using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Department : BaseDomainEntity
{
      public string Name { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public Guid CompanyGuid {get; private set;} 
      public List<Position> Positions { get; private set; } = new List<Position>();

      public Department(Guid guid, string name, string description,Guid companyGuid, int locationId,bool isActive,bool IsDefault) : base(guid,locationId,isActive,IsDefault)
      {
            ValidationHelper.IsNullOrEmpty(name, nameof(name));
            ValidationHelper.ValidateNotMinus(locationId, nameof(locationId));
            ValidationHelper.ValidateGuid(CompanyGuid,nameof(CompanyGuid));
            Name = name;
            Description = description;
            CompanyGuid = companyGuid;
      }
}
