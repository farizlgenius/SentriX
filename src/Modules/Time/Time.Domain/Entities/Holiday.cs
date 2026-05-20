using SharedKernel.Domain;

namespace Time.Domain.Entities;

public sealed class Holiday : BaseDomain
{
       public string Name { get; set; } = string.Empty;
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public string Metadata {get; set;} = string.Empty;
      //   public short Extend { get; set; }
      //   public short TypeMask { get; set; }

      public Holiday(int Id,short ComponentId,string Name,short Year,short Month,short Day,int LocationId,bool IsActive) : base(Id,ComponentId,LocationId,IsActive)
      {
            this.Name = Name;
            this.ComponentId = ComponentId;
            this.Year = Year;
            this.Month = Month;
            this.Day = Day;
            this.LocationId = LocationId;
            this.IsActive = IsActive;
      }
      
}