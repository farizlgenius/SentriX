namespace Core.Infrastructure.Persistences.Entities;

public sealed class Company : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public string address { get; set; } = string.Empty;
      public ICollection<User> users { get; set; } = new List<User>();
      public ICollection<Department> departments { get; set; } = new List<Department>();

      public Company(){}
      public Company(Core.Domain.Entities.Company d) : base(d.Guid)
      {
            this.name = d.Name;
            this.description = d.Description;
            this.address = d.Address;
      }
}