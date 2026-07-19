using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Pin
{
      public Guid Guid { get; private set; }
      public string Pins { get; private set;  } = string.Empty;
      public Guid UserGuid { get; private set; }

      public Pin(){}

      public Pin(
            Guid guid,
            string pins,
            Guid userGuid
            )
      {
            ValidationHelper.ValidateGuid(guid,nameof(Guid));
            ValidationHelper.IsNullOrEmpty(pins,nameof(Pins));
            ValidationHelper.ValidateGuid(userGuid,nameof(UserGuid));
            this.Guid = guid;
            this.Pins = pins;

      }

}