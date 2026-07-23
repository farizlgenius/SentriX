namespace Adapter.Amico.Persistences.Entities;

public sealed class Group : BaseSlot
{
      public Guid access_rule_guid { get; set; }
      public AccessRule access_rule { get; set; } = default!;
      public Group(
            Guid guid, 
            int slot
            ) : base(guid, slot)
      {
            this.access_rule_guid = guid;
      }
}