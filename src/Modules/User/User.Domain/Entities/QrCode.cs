using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class QrCode
{
      public Guid Guid { get; private set; }
      public string Qr { get; private set;  } = string.Empty;
      public Guid UserGuid { get; set; }

      public QrCode(){}

      public QrCode(
            Guid guid,
            string qr,
            Guid credentialGuid
            )
      {
            ValidationHelper.ValidateGuid(guid,nameof(Guid));
            ValidationHelper.ValidateGuid(credentialGuid,nameof(UserGuid));
            ValidationHelper.IsNullOrEmpty(qr,nameof(Qr));
            this.Guid = guid;
            this.Qr = qr;
            this.UserGuid = credentialGuid;
      }

}