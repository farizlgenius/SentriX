using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Credential : BaseDomain
{
      public short Flag { get; private set; }
      public long CardNumber { get; private set; }
      public int IssueCode { get; private set; }
      public string Pin { get; private set; } = string.Empty;
      public short UseCount { get; private set; }
      public DateTime ActiveTime { get; private set; }
      public DateTime DeactiveTime { get; private set; }
      public int UserId {get; private set;}
      public Credential(
            int id, 
            int userId,
            short flag,
            long cardNumber,
            int issueCode,
            string pin,
            short useCount,
            DateTime active,
            DateTime expire,
            int locationId, 
            bool IsActive
            ) : base(id, 0, locationId, IsActive)
      {
            ValidationHelper.ValidateNotMinus(flag,nameof(Flag));
            ValidationHelper.ValidateNotMinus(userId,nameof(UserId));
            ValidationHelper.ValidateNotMinus((int)CardNumber,nameof(CardNumber));
            ValidationHelper.ValidateNotMinus(IssueCode,nameof(IssueCode));
            ValidationHelper.ValidateDigit(Pin,nameof(Pin));
            if(active < DateTime.UtcNow)
                  throw new ArgumentException("Active date invalid.");

            if(expire < DateTime.UtcNow || expire < active)
                  throw new ArgumentException("Deactive date invalid.");

            this.Flag = flag;
            this.UserId = userId;
            this.CardNumber = cardNumber;
            this.IssueCode = issueCode;
            this.Pin = pin;
            this.UseCount = useCount;
            this.ActiveTime = active;
            this.DeactiveTime = expire;
              
      }
}
