using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class QrCode : BaseDomain
{
      public string QrCodes { get; private set; } = string.Empty;
      public int UserId { get; private set; } = default!;
      public QrCode(
            string Qr,
            int UserId
      )
      {
            ValidationHelper.IsNullOrEmpty(Qr, nameof(QrCodes));
            ValidationHelper.NotMinus(UserId, nameof(this.UserId));
            this.QrCodes = Qr;
            this.UserId = UserId;
      }

      public QrCode(
            Guid Guid,
            string Qr,
            int UserId
      ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(Qr, nameof(QrCodes));
            ValidationHelper.NotMinus(UserId, nameof(this.UserId));
            this.QrCodes = Qr;
            this.UserId = UserId;
      }
}