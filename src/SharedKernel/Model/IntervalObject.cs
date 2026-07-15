namespace SharedKernel.Model;

public sealed class IntervalObject
{
      public short ComponentId { get; set; }
      public int Start { get; set; }
      public int End { get; set; }
      public bool Sun { get; set; }
      public bool Mon { get; set; }
      public bool Tue { get; set; }
      public bool Wed { get; set; }
      public bool Thu { get; set; }
      public bool Fri { get; set; }
      public bool Sat { get; set; }


      public IntervalObject(short componentId, int start, int end, bool sun, bool mon, bool tue, bool wed, bool thu, bool fri, bool sat)
      {
            ComponentId = componentId;
            Start = start;
            End = end;
            Sun = sun;
            Mon = mon;
            Tue = tue;
            Wed = wed;
            Thu = thu;
            Fri = fri;
            Sat = sat;
      }
}

