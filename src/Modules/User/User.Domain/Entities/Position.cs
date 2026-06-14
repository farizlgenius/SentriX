using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Position : BaseDomain
{
       public string Name { get; private set; } = string.Empty;
      public string Description { get; private set; } = string.Empty;
      public int DepartmentId {get; private set;}

      public Position(int id, string name, string description,int departmentId, int locationId,bool isActive) : base(id,0,locationId,isActive)
      {
            ValidationHelper.IsNullOrEmpty(name, nameof(name));
            ValidationHelper.ValidateNotMinus(locationId, nameof(locationId));
            ValidationHelper.ValidateNotMinus(departmentId,nameof(DepartmentId));
            Name = name;
            Description = description;
            DepartmentId = departmentId;
      }
}
