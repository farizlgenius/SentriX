namespace Adapter.Amico.Persistences.Entities;

public sealed class AccessRule : BaseSlot
{
      public Group group { get; set; } = default!;
      public AccessRule(Guid guid, int slot) : base(guid, slot)
      {
      }
}