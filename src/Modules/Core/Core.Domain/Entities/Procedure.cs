namespace Core.Domain.Entities;

public sealed class Procedure : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public IEnumerable<int> ActionIds {get; private set;} = default!;
      public int DeviceId {get; private set;}
      public int LocationId {get; private set;}
      public Procedure(
            string name,
            int deviceId,
            int locationId,
            IEnumerable<int> actions,
            short externalId
      ) : base(Guid.NewGuid(),externalId)
      {
            Name = name;
            DeviceId = deviceId;
            ActionIds = actions;
            LocationId = locationId;
      }

}