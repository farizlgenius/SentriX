using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class LicensePlate : BaseDomain
{
      public string LicensePlates { get; private set; } = string.Empty;
      public LicensePlate(
            string Plate
      )
      {
            ValidationHelper.IsNullOrEmpty(Plate, nameof(LicensePlates));
            this.LicensePlates = Plate;
      }
      public LicensePlate(
            Guid Guid,
            string Plate
            ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(Plate, nameof(LicensePlates));
            this.LicensePlates = Plate;
      }
}