using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Area : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public AreaMultiOccMode MultiOccMode {get; private set; }
      public AreaMode AreaMode {get; private set;}
      public short OccMax {get; private set;}
      public short OccDown {get; private set; }
      public short OccUp {get; private set; }
      public string Metadata {get; private set;} = string.Empty;
      public int DeviceId {get; private set; } = 0;
      public List<int> AreaInDoors {get; private set; } = default!;
      public List<int> AreaOutDoors {get; private set; } =default!;
      public Area(
            string name,
            AreaMultiOccMode multiMode,
            AreaMode mode,
            short occMax,
            short occDown,
            short occUp,
            string metadata,
            List<int> areaInDoors,
            List<int> areaOutDoots,
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
            AreaInDoors = areaInDoors;
            AreaOutDoors = areaOutDoots;
      }

      public Area(
            Guid guid,
            string name,
            AreaMultiOccMode multiMode,
            AreaMode mode,
            short occMax,
            short occDown,
            short occUp,
            string metadata,
            List<int> areaInDoors,
            List<int> areaOutDoots
            ) : base(guid)
      {
            Name = name;
            MultiOccMode = multiMode;
            AreaMode = mode;
            OccMax = occMax;
            OccDown = occDown;
            OccUp = occUp;
            Metadata = metadata;
            AreaInDoors = areaInDoors;
            AreaOutDoors = areaOutDoots;
      }
}