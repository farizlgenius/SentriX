namespace Core.Domain.Entities;

public sealed class QrCode : BaseDomain
{
      public string QrCodes {get; private set;} = string.Empty;
      public Guid UserGuid {get; private set;} = default!;
      public QrCode(
            string Qr,
            Guid UserGuid
      )
      {
            this.QrCodes = Qr;
            this.UserGuid = UserGuid;
      }

      public QrCode(
            Guid Guid,
            string Qr,
            Guid UserGuid
      ) : base(Guid)
      {
            this.QrCodes = Qr;
            this.UserGuid = UserGuid;
      }
}