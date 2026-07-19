using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class LicensePlate 
{

      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public string license_plate { get; set; } = string.Empty;
      public DateTime created_at {get; set;}
      public DateTime updated_at {get; set;}
      public Guid? user_guid { get; set; }
      public Users user { get; set; } = default!;

      public LicensePlate(){}

      public LicensePlate(Domain.Entities.LicensePlate d)
      {
            this.guid = d.Guid;
            this.license_plate = d.LicensePlates;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
            this.user_guid = d.UserGuid;
      }

      public void Update(Domain.Entities.LicensePlate d)
      {
            this.license_plate = d.LicensePlates;
            this.updated_at = DateTime.UtcNow;
      }
}