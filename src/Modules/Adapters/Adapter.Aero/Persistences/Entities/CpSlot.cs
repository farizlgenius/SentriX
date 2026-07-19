using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class CpSlot : BaseSlot
{


      public CpSlot(){}
      public CpSlot(Guid scpGuid,int component_id) : base(scpGuid,component_id)
      {

      }
      

}
