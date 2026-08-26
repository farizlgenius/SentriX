using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Pin : BaseDomain
{
      public string Pins { get; private set; } = string.Empty;
      public int UserId { get; private set; } = default!;

      public Pin(
            string Pin,
            int UserId
      )
      {
            ValidationHelper.IsNullOrEmpty(Pin, nameof(Pins));
            ValidationHelper.NotMinus(UserId, nameof(this.UserId));
            this.Pins = Pin;
            this.UserId = UserId;
      }

      public Pin(
            Guid Guid,
            string Pin,
            int UserId
      ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(Pin, nameof(Pins));
            ValidationHelper.NotMinus(UserId, nameof(this.UserId));
            this.Pins = Pin;
            this.UserId = UserId;
      }
}