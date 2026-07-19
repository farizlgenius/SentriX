using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class Aeros : BaseEntity
{
      public int scp_id {get; set;}
      public string mac {get; set;} = string.Empty;


      public Aeros(){}
      public Aeros(int scp_id,string mac,int location_id)
      {
            this.scp_id = scp_id;
            this.mac = mac;
            this.location_id = location_id;
            this.updated_at = DateTime.UtcNow;
            this.created_at = DateTime.UtcNow;
      }
      

}
