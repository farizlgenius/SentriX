using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Time.Domain.Entities;

public sealed class Holiday : BaseDomainEntity
{
      public string Name { get; private set; } = string.Empty;
      public DateTime Start { get; private set; }
      public DateTime End { get; private set; }


      public Holiday(
            Guid Guid,
            string Name,
            DateTime Start,
            DateTime End,
            int LocationId,
            bool IsActive,
            bool IsDefault = false
            ) : base(Guid, LocationId, IsActive, IsDefault)
      {
            ValidationHelper.ValidateDateTime(nameof(DateTime), Start);
            ValidationHelper.ValidateDateTime(nameof(DateTime), End);
            this.Name = Name;
            this.Start = Start;
            this.End = End;
      }

}