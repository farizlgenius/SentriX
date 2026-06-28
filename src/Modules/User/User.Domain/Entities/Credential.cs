using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Credential : BaseDomain
{
      public short Flag { get; private set; }
      public short Bits {get; private set;}
      public short Fac {get; private set;}
      public long CardNumber { get; private set; }
      public int IssueCode { get; private set; }
      public string Pin { get; private set; } = string.Empty;
      public short UseCount { get; private set; }
      public short ApbLoc {get; private set;}
      public DateTime ActiveTime { get; private set; }
      public DateTime DeactiveTime { get; private set; }
      public int UserId {get; private set;}
      public Credential(
            int id, 
            int userId,
            short flag,
            short bits,
            short fac,
            long cardNumber,
            int issueCode,
            string pin,
            short useCount,
            short apbLoc,
            DateTime active,
            DateTime expire,
            int locationId, 
            bool IsActive
            ) : base(id, 0, locationId, IsActive)
      {
            ValidationHelper.ValidateNotMinus(flag,nameof(Flag));
            ValidationHelper.ValidateNotMinus(bits,nameof(Bits));
            ValidationHelper.ValidateNotMinus(userId,nameof(UserId));
            ValidationHelper.ValidateNotMinus((int)cardNumber,nameof(CardNumber));
            ValidationHelper.ValidateNotMinus(issueCode,nameof(IssueCode));
            ValidationHelper.ValidateDigit(pin,nameof(Pin));

            if(expire < DateTime.UtcNow)
                  throw new ArgumentException("Deactive date invalid.");

            if(active > expire)
                  throw new ArgumentException("Deactive date must larger than active.");

            this.Flag = flag;
            this.Bits = bits;
            this.Fac = fac;
            this.UserId = userId;
            this.CardNumber = cardNumber;
            this.IssueCode = issueCode;
            this.Pin = pin;
            this.UseCount = useCount;
            this.ApbLoc = apbLoc;
            this.ActiveTime = active;
            this.DeactiveTime = expire;
              
      }
}
