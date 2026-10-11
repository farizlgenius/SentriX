using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Actions : BaseDomain
{
      public ActionType Type {get; private set;}
      public string Metadata {get; private set;} = string.Empty;
      public int DeviceId {get; private set;}
      public Actions(
            ActionType type,
            string metadata,
            int deviceId
      ) : base(Guid.NewGuid())
      {
            Type = type;
            Metadata = metadata;
            DeviceId = deviceId;
      }     
}