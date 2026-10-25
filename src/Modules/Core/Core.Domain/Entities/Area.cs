using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Domain : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public AreaMultiOccMode MultiOccMode {get; private set; }
      public AreaMode AreaMode {get; private set;}
      public short OccMax {get; private set;}
      public short OccDown {get; private set; }
      public short OccUp {get; private set; }
      public string Metadata {get; private set;} = string.Empty;
      public int DeviceId {get; private set; } = 0;
      public Domain(
            string name,
            AreaMultiOccMode multiMode,
            AreaMode mode,
            short occMax,
            short occDown,
            short occUp,
            string metadata,
            int deviceId,
            short externalId
      ) : base(Guid.NewGuid(),externalId)
      {
            Name = name;
            MultiOccMode = multiMode;
            AreaMode = mode;
            OccMax = occMax;
            OccDown = occDown;
            OccUp = occUp;
            Metadata = metadata;
            DeviceId = deviceId;
      }

      public Domain(
            Guid guid,
            string name,
            AreaMultiOccMode multiMode,
            AreaMode mode,
            short occMax,
            short occDown,
            short occUp,
            string metadata
            ) : base(guid)
      {
            Name = name;
            MultiOccMode = multiMode;
            AreaMode = mode;
            OccMax = occMax;
            OccDown = occDown;
            OccUp = occUp;
            Metadata = metadata;
      }
}