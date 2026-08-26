using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class QrCode : BaseEntity
{
      public string qr_code { get; set; } = string.Empty;
      public int user_id { get; set; } = default!;
      public User user { get; set; } = default!;

      public QrCode() { }

      public QrCode(Domain.Entities.QrCode d) : base(d.Guid)
      {
            this.qr_code = d.QrCodes;
            this.user_id = d.UserId;
      }

}