using System;
using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class TzSlot : CentralBaseSlot
{

      public Guid? tz_guid { get; set; }

      public TzSlot() { }
      public TzSlot(int component_id) : base(component_id)
      {

      }



}
