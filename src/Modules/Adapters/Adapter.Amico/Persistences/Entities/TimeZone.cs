namespace Adapter.Amico.Persistences.Entities;

public sealed class TimeZone : BaseSlot
{
      public TimeZone(Guid guid, int slot) : base(guid, slot)
      {
      }

      public TimeZone(Guid guid,Guid device, int slot) : base(guid,device, slot)
      {
      }
}