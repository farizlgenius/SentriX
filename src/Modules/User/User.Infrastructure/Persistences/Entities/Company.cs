using System;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class Company : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public string address { get; set; } = string.Empty;
      public ICollection<Users> users { get; set; } = new List<Users>();
      public ICollection<Department> departments { get; set; } = new List<Department>();

      public Company() { }


      public Company(Domain.Entities.Company company) : base(0, company.LocationId, company.IsActive)
      {
            this.name = company.Name;
            this.address = company.Address;
            this.description = company.Description;

      }

      public void Update(Domain.Entities.Company company)
      {
            this.name = company.Name;
            this.address = company.Address;
            this.description = company.Description;
            this.updated_at = DateTime.UtcNow;
      }



}
