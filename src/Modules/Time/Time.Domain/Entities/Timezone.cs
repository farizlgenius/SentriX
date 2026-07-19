using SharedKernel.Domain;
using Time.Contract.DTOs;

namespace Time.Domain.Entities;

public sealed class TimeZone : BaseDomainEntity
{
       public string Name { get; private set; } = string.Empty;
        public List<Interval> Intervals {get; set;} = new List<Interval>();
      public TimeZone(
            Guid guid,
            string name,
            List<Interval> intervals,
            int locationId,
            bool isActive,
            bool isDefault
      ) : base(guid,locationId,isActive,isDefault)
      {
            this.Name = name;
            this.Intervals = intervals;
      }
      
}