using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class QrCode 
{

      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public string qr_code { get; set; } = string.Empty;
      public DateTime created_at {get; set;}
      public DateTime updated_at {get; set;}
      public Guid? user_guid { get; set; }
      public Users user { get; set; } = default!;

      public QrCode(){}

      public QrCode(Domain.Entities.QrCode d)
      {
            this.guid = d.Guid;
            this.qr_code = d.Qr;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
            this.user_guid = d.UserGuid;
      }

      public void Update(Domain.Entities.QrCode d)
      {
            this.qr_code = d.Qr;
            this.updated_at = DateTime.UtcNow;
      }
}