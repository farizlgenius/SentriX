using SharedKernel.Domain;
using Time.Contract.DTOs;

namespace Time.Domain.Entities;

public sealed class TimeZone : BaseDomainEntity
{
       public string Name { get; private set; } = string.Empty;
        public short Mode { get; private set; }
        public string Type { get; private set; }= string.Empty;
        public string Active { get; private set; }= string.Empty;
         public string Deactive { get; private set; }= string.Empty;
        public List<Interval> Intervals {get; set;} = new List<Interval>();
      public TimeZone(
            Guid guid,
            short componentId,
            string name,
            short mode,
            string type,
            string active,
            string deactive,
            List<Interval> intervals,
            int locationId,
            bool isActive,
            bool isDefault
      ) : base(guid,componentId,locationId,isActive,isDefault)
      {
            this.Name = name;
            this.Mode = mode;
            this.Type = type;
            this.Active = active;
            this.Deactive = deactive;
            this.Intervals = intervals;
      }
      
}