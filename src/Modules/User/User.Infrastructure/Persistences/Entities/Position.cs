using System;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class Position : BaseDbEntity
{
      public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public Guid department_guid {get; set;}
      public Department department {get; set;} = default!;
      public ICollection<Users> users { get; set; } = new List<Users>();

      public Position() { }


      public Position(Domain.Entities.Position position) : base(position.Guid,0, position.LocationId, position.IsActive,position.IsDefault)
      {
            this.name = position.Name;
            this.description = position.Description;
            this.department_guid = position.DepartmentGuid;
      }

      public void Update(Domain.Entities.Position position)
      {
            this.name = position.Name;
            this.description = position.Description;
            this.updated_at = DateTime.UtcNow;
      }
}
