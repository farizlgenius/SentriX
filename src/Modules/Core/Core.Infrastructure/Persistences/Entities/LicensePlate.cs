using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class LicensePlate : BaseEntity
{

      public string license_plate { get; set; } = string.Empty;
      public Guid? user_guid { get; set; }
      public User user { get; set; } = default!;

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