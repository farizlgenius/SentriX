using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class Pin 
{

      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public string pin { get; set; } = string.Empty;
      public DateTime created_at {get; set;}
      public DateTime updated_at {get; set;}
      public Guid? user_guid { get; set; }
      public Users user { get; set; } = default!;

      public Pin(){}

      public Pin(Domain.Entities.Pin d)
      {
            this.guid = d.Guid;
            this.pin = d.Pins;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
            this.user_guid = d.UserGuid;
      }

      public void Update(Domain.Entities.Pin d)
      {
            this.pin = d.Pins;
            this.updated_at = DateTime.UtcNow;
      }
}