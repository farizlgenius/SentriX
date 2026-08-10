namespace Core.Infrastructure.Persistences.Entities;

public sealed class Position : BaseEntity
{
      public string name {get; set;} = string.Empty;
      public string description { get; set; } = string.Empty;
      public Guid department_guid { get; set; }
      public Department department { get; set; } = default!;
      public ICollection<User> users { get; set; } = new List<User>();
      public Position(){} 
      public Position(Core.Domain.Entities.Position d) : base(d.Guid)
      {
            this.name = d.Name;
            this.description = d.Description;
            this.department_guid = d.DepartmentGuid;
      }
}