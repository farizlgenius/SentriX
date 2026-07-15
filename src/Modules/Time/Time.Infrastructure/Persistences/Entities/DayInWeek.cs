using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Time.Infrastructure.Persistences.Entities;

public sealed class DayInWeek 
{
      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public bool sunday { get; set; }
      public bool monday { get; set; }
      public bool tuesday { get; set; }
      public bool wednesday { get; set; }
      public bool thursday { get; set; }
      public bool friday { get; set; }
      public bool saturday { get; set; }
      public Guid interval_guid {get; set;}
      public Interval interval {get; set;} = default!;
      public DayInWeek()
      {
      }

      public DayInWeek(
      Guid guid,
      bool sunday,
      bool monday,
      bool tuesday,
      bool wednesday,
      bool thursday,
      bool friday,
      bool saturday
      ) 
      {
            this.guid = guid;
            this.sunday = sunday;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday =saturday;
      }
}