using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Card
{
      public Guid Guid { get; private set; } = Guid.Empty;
      public short Bits { get; private set; } = 0;
      public int CardNumber { get; private set; } = -1;
      public Guid UserGuid { get; private set; }

      public Card(){}

      public Card(
            Guid guid,
            short bits,
            int cardNumber,
            Guid userGuid
            )
      {
            ValidationHelper.ValidateGuid(guid,nameof(Guid));
            ValidationHelper.ValidateNotMinus(bits,nameof(Bits));
            ValidationHelper.ValidateNotMinus(cardNumber,nameof(CardNumber));
            ValidationHelper.ValidateGuid(userGuid,nameof(UserGuid));
            this.Guid = guid;
            this.Bits = bits;
            this.CardNumber = cardNumber;
            this.UserGuid = userGuid;
      }

}