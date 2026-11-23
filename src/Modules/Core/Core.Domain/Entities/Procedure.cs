namespace Core.Domain.Entities;

public sealed class Procedure : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public IEnumerable<Domain.Entities.Action> Actions {get; private set;} = default!;
      public int DeviceId {get; private set;}
      public int LocationId {get; private set;}
      public Procedure(
            string name,
            int deviceId,
            int locationId,
            IEnumerable<Domain.Entities.Action> actions,
            short externalId
      ) : base(Guid.NewGuid(),externalId)
      {
            Name = name;
            DeviceId = deviceId;
            Actions = actions;
            LocationId = locationId;
      }

      public Procedure(
            Guid guid,
            string name,
            int deviceId,
            int locationId,
            IEnumerable<Domain.Entities.Action> actions,
            short externalId
      ) : base(guid,externalId)
      {
            Name = name;
            DeviceId = deviceId;
            Actions = actions;
            LocationId = locationId;
      }


}