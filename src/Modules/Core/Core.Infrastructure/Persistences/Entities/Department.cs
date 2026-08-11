namespace Core.Infrastructure.Persistences.Entities;

public sealed class Department : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;

      // Relation
      public Guid company_guid { get; set; }
      public Company company { get; set; } = default!;
      public ICollection<User> users { get; set; } = default!;
      public ICollection<Position> positions { get; set; } = default!;

      public Department() { }
      public Department(Core.Domain.Entities.Department d) : base(d.Guid)
      {
            this.name = d.Name;
            this.description = d.Description;
            this.company_guid = d.CompanyGuid;
      }
}