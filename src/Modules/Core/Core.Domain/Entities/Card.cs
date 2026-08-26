using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Card : BaseDomain
{
      public short Bits { get; private set; }
      public int Fac { get; private set; }
      public int CardNumber { get; private set; }
      public int UserId { get; private set; }

      public Card(short bits, int fac, int cardNumber, int userId)
      {
            ValidationHelper.NotMinus(bits, nameof(Bits));
            ValidationHelper.NotMinus(fac, nameof(Fac));
            ValidationHelper.NotMinus(cardNumber, nameof(CardNumber));
            ValidationHelper.NotMinus(userId, nameof(UserId));
            Bits = bits;
            Fac = fac;
            CardNumber = cardNumber;
            UserId = userId;
      }

      public Card(Guid guid, short bits, int fac, int cardNumber, int userId) : base(guid)
      {
            ValidationHelper.NotMinus(bits, nameof(Bits));
            ValidationHelper.NotMinus(fac, nameof(Fac));
            ValidationHelper.NotMinus(cardNumber, nameof(CardNumber));
            ValidationHelper.NotMinus(userId, nameof(UserId));
            Bits = bits;
            Fac = fac;
            CardNumber = cardNumber;
            UserId = userId;
      }

}