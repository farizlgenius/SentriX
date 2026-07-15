using SharedKernel.Domain;

namespace Time.Domain.Entities;

public sealed class Interval 
{
      public Guid Guid { get; private set; }
      public short ComponentId {get; private set;}
      public DayInWeek Days { get; private set; }
      public string DaysDetail { get; private set; } = string.Empty;
      public string Start { get; private set; } = string.Empty;
      public string End { get; set; } = string.Empty;
      public Interval(
            Guid Guid,
            short ComponentId,
            DayInWeek Days,
            string DaysDetail,
            string Start,
            string End
            ) 
      {
            this.Guid = Guid;
            this.ComponentId = ComponentId;
            this.Days = Days;
            this.DaysDetail = DaysDetail;
            this.Start = Start;
            this.End = End;
      }
}