using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Position : BaseDomainEntity
{
       public string Name { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public Guid DepartmentGuid {get; private set;}

      public Position(Guid guid, string name, string description,Guid departmentGuid, int locationId,bool isActive,bool IsDefault) : base(guid,0,locationId,isActive,IsDefault)
      {
            ValidationHelper.IsNullOrEmpty(name, nameof(name));
            ValidationHelper.ValidateNotMinus(locationId, nameof(locationId));
            ValidationHelper.ValidateGuid(departmentGuid,nameof(DepartmentGuid));
            Name = name;
            Description = description;
            DepartmentGuid = departmentGuid;
      }
}
