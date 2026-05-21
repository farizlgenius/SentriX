using SharedKernel.Domain;

namespace Time.Infrastructure.Persistences.Entities;

public sealed class DayInWeek : BaseEntity
{
      public bool sunday { get; set; }
      public bool monday { get; set; }
      public bool tuesday { get; set; }
      public bool wednesday { get; set; }
      public bool thursday { get; set; }
      public bool friday { get; set; }
      public bool saturday { get; set; }
      public int interval_id {get; set;}
      public Interval interval {get; set;} = default!;
      public DayInWeek()
      {
      }

      public DayInWeek(short componetId,
      bool sunday,
      bool monday,
      bool tuesday,
      bool wednesday,
      bool thursday,
      bool friday,
      bool saturday,
       int locationId, bool isactive) : base(componetId, locationId, isactive)
      {
            this.sunday = sunday;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday =saturday;
      }
}