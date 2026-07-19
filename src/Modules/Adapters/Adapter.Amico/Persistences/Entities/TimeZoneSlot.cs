using SharedKernel.Domain;

namespace Adapter.Amico.Persistences.Entities;

public sealed class TimeZoneSlot : BaseSlot
{
      public TimeZoneSlot(){}
      public TimeZoneSlot(Guid guid,int slot) : base(guid,slot){}
}