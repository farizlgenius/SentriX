using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class QrCode : BaseDomain
{
      public string QrCodes { get; private set; } = string.Empty;
      public QrCode(
            string Qr
      )
      {
            ValidationHelper.IsNullOrEmpty(Qr, nameof(QrCodes));
            this.QrCodes = Qr;
      }

      public QrCode(
            Guid Guid,
            string Qr
      ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(Qr, nameof(QrCodes));
            this.QrCodes = Qr;
      }
}