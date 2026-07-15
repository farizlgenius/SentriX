using System.ComponentModel;
using SharedKernel.Domain;

namespace Time.Infrastructure.Persistences.Entities;

public sealed class Holiday : BaseDbEntity
{
      public string name { get; set; } = string.Empty;
      public DateTime start { get; set; }
      public DateTime end { get; set; }

      public Holiday() { }


      public Holiday(Guid guid,short componentId,string name,DateTime start,DateTime end, int location, bool is_active,bool is_default) : base(guid,componentId,location, is_active,is_default)
      {
            this.guid = guid;
            this.name = name;
            this.start = start;
            this.end = end;
      }

      public void Update(Domain.Entities.Holiday domain)
      {
            this.name = domain.Name;
            this.start = domain.Start;
            this.end = domain.End;
            this.updated_at = DateTime.UtcNow;
      }

}