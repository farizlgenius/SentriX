using System;
using System.ComponentModel.DataAnnotations;
using Adapter.Aero.Model;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class GroupSlot : BaseSlot
{
      public GroupSlot() { }
      public GroupSlot(int component_id) : base(component_id)
      {

      }



}
