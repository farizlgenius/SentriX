using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Pin : BaseDomain
{
      public string Pins { get; private set; } = string.Empty;
      public Guid UserGuid { get; private set; } = default!;

      public Pin(
            string Pin,
            Guid UserGuid
      )
      {
            ValidationHelper.IsNullOrEmpty(Pin, nameof(Pins));
            ValidationHelper.GuidEmpty(UserGuid, nameof(this.UserGuid));
            this.Pins = Pin;
            this.UserGuid = UserGuid;
      }

      public Pin(
            Guid Guid,
            string Pin,
            Guid UserGuid
      ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(Pin, nameof(Pins));
            ValidationHelper.GuidEmpty(UserGuid, nameof(this.UserGuid));
            this.Pins = Pin;
            this.UserGuid = UserGuid;
      }
}