using SharedKernel.Domain;

namespace Time.Infrastructure.Persistences.Entities;

public sealed class Interval : BaseEntity
{
      public int day_in_week_id {get; set;}
      public DayInWeek days { get; set; } = default!;
      public string days_detail { get; set; } = string.Empty;
      public string start { get; set; } = string.Empty;
      public string end { get; set; } = string.Empty;
      public int timezone_id {get; set;}
      public Timezone timezone {get; set;} = default!;
      public Interval()
      {
      }

      public Interval(short componetId,DayInWeek days,string start,string end, int locationId, bool isactive) : base(componetId, locationId, isactive)
      {
            this.days = days;
            this.days_detail ="";
            this.start = start;
            this.end = end;
      }
}