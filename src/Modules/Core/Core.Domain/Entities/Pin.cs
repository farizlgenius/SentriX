using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Pin : BaseDomain
{
      public string Pins { get; private set; } = string.Empty;

      public Pin(
            string Pin
      )
      {
            ValidationHelper.IsNullOrEmpty(Pin, nameof(Pins));
            this.Pins = Pin;
      }

      public Pin(
            Guid Guid,
            string Pin,
            int UserId
      ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(Pin, nameof(Pins));
            this.Pins = Pin;
      }
}