using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class LicensePlate
{
      public Guid Guid { get; private set; }
      public string LicensePlates { get; private set;  } = string.Empty;
      public Guid UserGuid { get; private set; }

      public LicensePlate(){}

      public LicensePlate(
            Guid guid,
            string license,
            Guid userGuid
            )
      {
            ValidationHelper.ValidateGuid(guid,nameof(Guid));
            ValidationHelper.ValidateGuid(userGuid,nameof(UserGuid));
            ValidationHelper.IsNullOrEmpty(license,nameof(LicensePlates));
            this.Guid = guid;
            this.LicensePlates = license;
            this.UserGuid = userGuid;
      }

}