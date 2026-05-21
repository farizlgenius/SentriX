using SharedKernel.Domain;

namespace Time.Infrastructure.Persistences.Entities;

public sealed class Timezone : BaseEntity
{
      public string name {get; set;} = string.Empty;
      public short mode {get; set;}
      public string active {get; set;} = string.Empty;
      public string deactive {get; set;} = string.Empty;
      public ICollection<Interval> intervals {get; set;} = default!;

      public Timezone()
      {
      }

      public Timezone(Domain.Entities.Timezone domain) : base(domain.ComponentId,domain.LocationId,domain.IsActive)
      {
            this.name = domain.Name;
            this.mode = domain.Mode;
            this.active = domain.Active;
            this.deactive = domain.Deactive;
            this.intervals = domain.Intervals.Select(x => new Interval(
                  x.ComponentId,
                  new DayInWeek(
                        x.Days.ComponentId,
                        x.Days.Sunday,
                        x.Days.Monday,
                        x.Days.Tuesday,
                        x.Days.Wednesday,
                        x.Days.Thursday,
                        x.Days.Friday,
                        x.Days.Saturday,
                        x.LocationId,
                        x.IsActive
                  ),
                  x.Start,
                  x.End,
                  x.LocationId,
                  x.IsActive
                  )).ToList();
      }

      public Timezone(short componetId,string name,short mode,string active,string deactive,List<Interval> intervals, int locationId, bool isactive) : base(componetId, locationId, isactive)
      {
            this.name = name;
            this.mode = mode;
            this.active = active;
            this.deactive = deactive;
      }
}