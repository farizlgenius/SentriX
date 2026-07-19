using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class AcrSlot : BaseSlot
{

      public AcrSlot(){}
      public AcrSlot(Guid scpGuid,int component_id) : base(scpGuid,component_id)
      {
      }
      

}
