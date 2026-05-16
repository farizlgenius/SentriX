using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class Scp : BaseEntity
{
      public int scp_id {get; set;}
      public string mac {get; set;} = string.Empty;
      public int location_id {get; set;} 

      public Scp(){}
      

}
