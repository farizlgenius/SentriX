using System;
using Adapter.Aero.Model;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class MpgSlot : BaseSlot
{


      public MpgSlot(){}
      public MpgSlot(Guid scpGuid,int component_id) : base(scpGuid,component_id)
      {
      }
      

}
