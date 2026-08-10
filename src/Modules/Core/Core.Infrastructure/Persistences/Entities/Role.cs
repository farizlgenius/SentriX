namespace Core.Infrastructure.Persistences.Entities;

public sealed class Role : BaseEntity
{
      public string name {get; set;} = string.Empty;

      // Releation
      public ICollection<User> users { get; set; } = default!;
      public ICollection<Permission> permissions { get; set; } = default!;

      public Role(){}
      public Role(Core.Domain.Entities.Role d) : base(d.Guid)
      {
            
      }
}