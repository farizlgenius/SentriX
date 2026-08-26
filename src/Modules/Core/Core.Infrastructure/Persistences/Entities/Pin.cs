using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Pin : BaseEntity
{

      public string pin { get; set; } = string.Empty;
      public int? user_id { get; set; }
      public User user { get; set; } = default!;

      public Pin() { }

      public Pin(Domain.Entities.Pin d)
      {
            this.guid = d.Guid;
            this.pin = d.Pins;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
            this.user_id = d.UserId;
      }

      public void Update(Domain.Entities.Pin d)
      {
            this.pin = d.Pins;
            this.updated_at = DateTime.UtcNow;
      }
}