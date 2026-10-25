namespace Core.Domain.Entities;

public sealed class MonitorGroup : BaseDomain
{
      public string Name {get; private set; } =string.Empty;
      public List<MonitorGroupComponent> Components {get; private set;} = new List<MonitorGroupComponent>();
      public int DeviceId {get; private set; } = 0;
      public MonitorGroup(
            string name,
            List<MonitorGroupComponent> components,
            int deviceId,
            short externalId
      ) : base(Guid.NewGuid(),externalId)
      {
            Name = name;
            Components = components;
            DeviceId = deviceId;
      }

      public MonitorGroup(
            Guid guid,
            string name,
            List<MonitorGroupComponent> components
      ) : base(guid)
      {
            Name = name;
            Components = components;
      }
}