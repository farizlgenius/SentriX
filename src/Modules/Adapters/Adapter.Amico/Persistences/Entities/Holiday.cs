namespace Adapter.Amico.Persistences.Entities;

public sealed class Holiday : BaseSlot
{

      public Holiday(Guid guid, Guid device_guid, int slot) : base(guid, device_guid, slot)
      {
      }
}