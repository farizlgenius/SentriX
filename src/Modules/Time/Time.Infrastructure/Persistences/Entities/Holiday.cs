using SharedKernel.Domain;

namespace Time.Infrastructure.Persistences.Entities;

public sealed class Holiday : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public short year { get; set; }
      public short month { get; set; }
      public short day { get; set; }
      public string metadata { get; set; } = string.Empty;
      //   public short extend { get; set; }
      //   public short type_mask { get; set; }

      public Holiday() { }


      public Holiday(short componentId, string name, short year, short month, short day, string metadata, int location, bool is_active) : base(componentId, location, is_active)
      {
            this.name = name;
            this.year = year;
            this.month = month;
            this.day = day;
            this.metadata = metadata;
      }

}