using SharedKernel.Domain;

namespace Time.Domain.Entities;

public sealed class Interval : BaseDomain
{
      public DayInWeek Days { get; private set; }
      public string DaysDetail { get; private set; } = string.Empty;
      public string Start { get; private set; } = string.Empty;
      public string End { get; set; } = string.Empty;
      public string Type { get; set; } = string.Empty;
      public Interval(
            int id, 
            short componentId, 
            int locationId, 
            bool IsActive,
            DayInWeek Days,
            string DaysDetail,
            string Start,
            string End,
            string Type
            ) : base(id, componentId, locationId, IsActive)
      {

            this.Days = Days;
            this.DaysDetail = DaysDetail;
            this.Start = Start;
            this.End = End;
            this.Type = Type;
      }
}