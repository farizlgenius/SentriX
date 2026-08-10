using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class LicensePlate : BaseDomain
{
      public string LicensePlates {get; private set;} = string.Empty;
      public Guid UserGuid {get; private set;}
      public LicensePlate(
            string Plate,
            Guid UserGuid
      )
      {
            ValidationHelper.IsNullOrEmpty(Plate,nameof(LicensePlates));
            ValidationHelper.ValidateGuid(UserGuid,nameof(this.UserGuid));
            this.LicensePlates = Plate;
            this.UserGuid = UserGuid;
      }
      public LicensePlate(
            Guid Guid,
            string Plate,
            Guid UserGuid
            ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(Plate,nameof(LicensePlates));
            ValidationHelper.ValidateGuid(UserGuid,nameof(this.UserGuid));
             this.LicensePlates = Plate;
            this.UserGuid = UserGuid;
      }
}