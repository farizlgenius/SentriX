using Group.Domain.Entities;
using SharedKernel.Domain;

namespace Group.Infrastructure.Persistences.Entities;

public sealed class Groups : BaseDbEntity
{
      public string name { get; set; } = string.Empty;
      public ICollection<GroupDoor> group_doors {get; set;} = default!;
      
      public Groups()
      {
            
      }

      public Groups(Domain.Entities.Groups domain) : base(domain.Guid,domain.ComponentId, domain.LocationId, domain.IsActive,false)
      {
            this.name = domain.Name;
            this.group_doors = domain.GroupDoors.Select(x => new GroupDoor(x)).ToList();
            this.created_at = DateTime.UtcNow;
            this.updated_at  = DateTime.UtcNow;
      }

      public void Update(Domain.Entities.Groups domain)
      {
            this.name = domain.Name;
            this.updated_at = DateTime.UtcNow;
      }
}