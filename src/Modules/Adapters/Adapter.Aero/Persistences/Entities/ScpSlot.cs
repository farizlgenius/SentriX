using System;
using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class ScpSlot : CentralBaseSlot
{

      public string mac {get ;set;} = string.Empty;

      public ScpSlot(){}
      public ScpSlot(int slot) : base(slot)
      {
            
      }

      public void Inserted(Guid device,string mac)
      {
            base.Inserted(device);
            this.mac = mac;
      }



}
