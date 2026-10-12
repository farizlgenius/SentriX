using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Actions : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public ActionType Type {get; private set;}
      public string Metadata {get; private set;} = string.Empty;
      public int DeviceId {get; private set;}
      public int LocationId {get; private set;}
      public Actions(
            string name,
            ActionType type,
            string metadata,
            int deviceId,
            int locationId
      ) : base(Guid.NewGuid())
      {
            Name = name;
            Type = type;
            Metadata = metadata;
            DeviceId = deviceId;
            LocationId = locationId;
      }     
}