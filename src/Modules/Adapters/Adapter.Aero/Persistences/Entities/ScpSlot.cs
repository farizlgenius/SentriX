using System;
using System.ComponentModel.DataAnnotations;
using Adapter.Aero.Model;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class ScpSlot : BaseSlot
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
