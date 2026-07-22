using System;
using System.ComponentModel.DataAnnotations;
using Adapter.Aero.Model;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class CardFormatSlot : BaseSlot
{
      public CardFormatSlot() { }
      public CardFormatSlot(int slot_id) : base(slot_id)
      {

      }



}
