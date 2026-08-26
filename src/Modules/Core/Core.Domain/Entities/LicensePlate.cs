using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class LicensePlate : BaseDomain
{
      public string LicensePlates { get; private set; } = string.Empty;
      public int UserId { get; private set; }
      public LicensePlate(
            string Plate,
            int UserId
      )
      {
            ValidationHelper.IsNullOrEmpty(Plate, nameof(LicensePlates));
            ValidationHelper.NotMinus(this.UserId, nameof(this.UserId));
            this.LicensePlates = Plate;
            this.UserId = UserId;
      }
      public LicensePlate(
            Guid Guid,
            string Plate,
            int UserId
            ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(Plate, nameof(LicensePlates));
            ValidationHelper.NotMinus(UserId, nameof(this.UserId));
            this.LicensePlates = Plate;
            this.UserId = UserId;
      }
}