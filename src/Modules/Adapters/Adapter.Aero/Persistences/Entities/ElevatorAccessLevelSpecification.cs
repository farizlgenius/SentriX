using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class ElevatorAccessLevelSpecification : BaseEntity
{
      public short scp_id {get; set;}
      public string mac {get; set;} = string.Empty;
      public short max_ealvl {get; set;}
      public short max_floors {get; set;}

      public ElevatorAccessLevelSpecification(){}

      public ElevatorAccessLevelSpecification(short scp_id, string mac, short max_ealvl, short max_floors)
      {
            this.scp_id = scp_id;
            this.mac = mac;
            this.max_ealvl = max_ealvl;
            this.max_floors = max_floors;
      }
}
