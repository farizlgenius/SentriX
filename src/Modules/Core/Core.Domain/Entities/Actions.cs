using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Actions : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public ActionType Type {get; private set;}
      public string? Metadata {get; private set;}
      public int DeviceId {get; private set;}
      public int LocationId {get; private set;}

      public Actions(
            string name,
            ActionType type,
            int deviceId,
            int locationId,
            short? externalId=null,
            string? metadata=null
      ) : base(Guid.NewGuid(),externalId)
      {
            Name = name;
            Type = type;
            Metadata = metadata;
            DeviceId = deviceId;
            LocationId = locationId;
      }

      public Actions(
            Guid guid,
            string name,
            ActionType type,
            int deviceId,
            int locationId,
            short? externalId=null,
            string? metadata=null
      ) : base(guid,externalId)
      {
            Name = name;
            Type = type;
            Metadata = metadata;
            DeviceId = deviceId;
            LocationId = locationId;
      }     
}