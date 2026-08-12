using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Card : BaseDomain
{
      public short Bits { get; private set; }
      public int Fac { get; private set; }
      public int CardNumber { get; private set; }
      public Guid UserGuid { get; private set; }

      public Card(short bits, int fac, int cardNumber, Guid userGuid)
      {
            ValidationHelper.NotMinus(bits, nameof(Bits));
            ValidationHelper.NotMinus(fac, nameof(Fac));
            ValidationHelper.NotMinus(cardNumber, nameof(CardNumber));
            ValidationHelper.GuidEmpty(userGuid, nameof(UserGuid));
            Bits = bits;
            Fac = fac;
            CardNumber = cardNumber;
            UserGuid = userGuid;
      }

      public Card(Guid guid, short bits, int fac, int cardNumber, Guid userGuid) : base(guid)
      {
            ValidationHelper.NotMinus(bits, nameof(Bits));
            ValidationHelper.NotMinus(fac, nameof(Fac));
            ValidationHelper.NotMinus(cardNumber, nameof(CardNumber));
            ValidationHelper.GuidEmpty(userGuid, nameof(UserGuid));
            Bits = bits;
            Fac = fac;
            CardNumber = cardNumber;
            UserGuid = userGuid;
      }

}