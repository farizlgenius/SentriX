using System;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class Department : BaseEntity
{
       public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public int company_id {get; set;}
      public Company company {get; set;} = default!;
      public ICollection<Users> users { get; set; } = new List<Users>();
      public ICollection<Position> positions { get; set; } = new List<Position>();

      public Department() { }


      public Department(Domain.Entities.Department department) : base(0, department.LocationId, department.IsActive,false)
      {
            this.name = department.Name;
            this.description = department.Description;
            this.company_id = department.CompanyId;
      }

      public void Update(Domain.Entities.Department department)
      {
            this.name = department.Name;
            this.description = department.Description;
            this.updated_at = DateTime.UtcNow;
      }
}
