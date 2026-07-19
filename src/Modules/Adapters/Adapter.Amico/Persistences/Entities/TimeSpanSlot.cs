using SharedKernel.Domain;

namespace Adapter.Amico.Persistences.Entities;

public sealed class TimeSpanSlot : BaseSlot
{
      public TimeSpanSlot(){}
      public TimeSpanSlot(Guid guid,int slot) : base(guid,slot){}
}