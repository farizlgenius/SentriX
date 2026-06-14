using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Department : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public int CompanyId {get; private set;} 
      public List<Position> Positions { get; private set; } = new List<Position>();

      public Department(int id, string name, string description,int companyId, int locationId,bool isActive) : base(id,0,locationId,isActive)
      {
            ValidationHelper.IsNullOrEmpty(name, nameof(name));
            ValidationHelper.ValidateNotMinus(locationId, nameof(locationId));
            ValidationHelper.ValidateNotMinus(companyId,nameof(CompanyId));
            Name = name;
            Description = description;
            CompanyId = companyId;
      }
}
