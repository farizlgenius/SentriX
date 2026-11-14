namespace Core.Domain.Entities;

public sealed class TriggerVariable : BaseDomain
{
      public int TriggerId {get; set; }
      public int DeviceId {get; set; }
      public TriggerVariable(
            int triggerId,
            int deviceId,
            short externalId
      ) : base(Guid.NewGuid(),externalId)
      {
            TriggerId = triggerId;
            DeviceId = deviceId;
      }

      public TriggerVariable(
            Guid guid,
            int triggerId,
            int deviceId,
            short externalId
      ) : base(guid,externalId)
      {
             TriggerId = triggerId;
            DeviceId = deviceId;
      }
}