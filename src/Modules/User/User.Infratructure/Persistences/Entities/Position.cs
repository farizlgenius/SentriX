using System;
using SharedKernel.Domain;

namespace User.Infratructure.Persistences.Entities;

public sealed class Position : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public ICollection<Users> users { get; set; } = new List<Users>();

      public Position() { }


      public Position(Domain.Entities.Position position) : base(0, position.LocationId, position.IsActive)
      {
            this.name = position.Name;
            this.description = position.Description;
      }

      public void Update(Domain.Entities.Position position)
      {
            this.name = position.Name;
            this.description = position.Description;
            this.updated_at = DateTime.UtcNow;
      }
}
