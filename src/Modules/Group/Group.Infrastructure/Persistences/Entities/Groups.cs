using SharedKernel.Domain;

namespace Group.Infrastructure.Persistences.Entities;

public sealed class Groups : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string metadata { get; set; } = string.Empty;
      
      public Groups()
      {
            
      }

      public Groups(Domain.Entities.Groups domain) : base(domain.ComponentId, domain.LocationId, domain.IsActive)
      {
            this.name = domain.Name;
            this.metadata = domain.Metadata;
            this.created_at = DateTime.UtcNow;
            this.updated_at  = DateTime.UtcNow;
      }

      public void Update(Domain.Entities.Groups domain)
      {
            this.name = domain.Name;
            this.metadata = domain.Metadata;
            this.updated_at = DateTime.UtcNow;
      }
}