namespace Core.Domain.Entities;

public sealed class Procedure : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public IEnumerable<(int id,int respective)> ActionIds {get; private set;} = default!;
      public int DeviceId {get; private set;}
      public int LocationId {get; private set;}
      public Procedure(
            string name,
            int deviceId,
            int locationId,
            IEnumerable<(int id,int sequence)> actions,
            short externalId
      ) : base(Guid.NewGuid(),externalId)
      {
            Name = name;
            DeviceId = deviceId;
            ActionIds = actions;
            LocationId = locationId;
      }

      public Procedure(
            Guid guid,
            string name,
            int deviceId,
            int locationId,
            IEnumerable<(int id,int sequence)> actions,
            short externalId
      ) : base(guid,externalId)
      {
            Name = name;
            DeviceId = deviceId;
            ActionIds = actions;
            LocationId = locationId;
      }

}