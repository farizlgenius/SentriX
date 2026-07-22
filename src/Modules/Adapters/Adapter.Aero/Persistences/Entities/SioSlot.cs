using System;
using Adapter.Aero.Model;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class SioSlot : BaseSlot
{
      public SioSlot(){}
      public SioSlot(Guid scpGuid,int slot_id) : base(scpGuid,slot_id)
      {
      }  

}
