using SharedKernel.Domain;

namespace Time.Domain.Entities;

public sealed class DayInWeek 
{
      public Guid Guid { get; private set; }
      public bool Sunday { get; private set; }
      public bool Monday { get; private set; }
      public bool Tuesday { get; private set; }
      public bool Wednesday { get; private set; }
      public bool Thursday { get; private set; }
      public bool Friday { get; private set; }
      public bool Saturday {get; private set;}

      public DayInWeek(
            Guid guid,
            bool Sunday,
            bool Monday, 
            bool Tuesday, 
            bool Wednesday, 
            bool Thursday, 
            bool Friday, 
            bool Saturday
            ) 
      {
            this.Guid = guid;
            this.Sunday = Sunday;
            this.Monday = Monday;
            this.Tuesday = Tuesday;
            this.Wednesday = Wednesday;
            this.Thursday = Thursday;
            this.Friday = Friday;
            this.Saturday = Saturday;
      }


}