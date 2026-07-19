using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Time.Infrastructure.Persistences.Entities;

public sealed class Interval 
{
      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public short component_id { get; set; }
      public string start { get; set; } = string.Empty;
      public string end { get; set; } = string.Empty;
      public Guid timezone_guid {get; set;}
      public TimeZone timezone {get; set;} = default!;
      public Guid day_in_week_guid {get; set;}
      public DayInWeek days { get; set; } = default!;
      public Interval()
      {
      }

      public Interval(Guid guid,short component_id,DayInWeek days,string start,string end) 
      {
            this.component_id = component_id;
            this.guid = guid;
            this.days = days;
            this.start = start;
            this.end = end;
      }
}