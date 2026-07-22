using System;
using Adapter.Aero.Model;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class MpSlot : BaseSlot
{

      public MpSlot(){}
      public MpSlot(Guid scpGuid,int component_id) : base(scpGuid,component_id)
      {

      }
      

}
