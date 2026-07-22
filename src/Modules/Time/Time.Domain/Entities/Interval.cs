using SharedKernel.Domain;

namespace Time.Domain.Entities;

public sealed class Interval 
{
      public Guid Guid { get; private set; }
      public DayInWeek Days { get; private set; }
      public string Start { get; private set; } = string.Empty;
      public string End { get; set; } = string.Empty;
      public Interval(
            Guid Guid,
            DayInWeek Days,
            string Start,
            string End
            ) 
      {
            this.Guid = Guid;
            this.Days = Days;
            this.Start = Start;
            this.End = End;
      }
}