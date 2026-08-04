namespace Adapter.Amico.Persistences.Entities;

public sealed class TimeSpan : BaseSlot
{
      public TimeSpan(Guid guid,int slot) : base(guid, slot)
      {
      }

      public TimeSpan(Guid guid,Guid device, int slot) : base(guid,device, slot)
      {
      }
}