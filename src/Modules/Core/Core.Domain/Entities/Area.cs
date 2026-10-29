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
      public List<(int doorId,AntipassBackMode antipassback,int area,DoorDirection direction,DoorType type)> DoorsIn {get; private set; } = default!;
      public List<(int doorId,AntipassBackMode antipassback,int area,DoorDirection direction,DoorType type)> DoorsOut {get; private set; } = default!;
      public int DeviceId {get; private set;}
      public Area(
            string name,
            AreaMultiOccMode multiMode,
            AreaMode mode,
            short occMax,
            short occDown,
            short occUp,
            string metadata,
            List<(int doorId,AntipassBackMode antipassback,int area,DoorDirection direction,DoorType type)> doorsIn,
            List<(int doorId,AntipassBackMode antipassback,int area,DoorDirection direction,DoorType type)> doorsOut,
            short externalId,
            int deviceId
      ) : base(Guid.NewGuid(),externalId)
      {
            Name = name;
            MultiOccMode = multiMode;
            AreaMode = mode;
            OccMax = occMax;
            OccDown = occDown;
            OccUp = occUp;
            Metadata = metadata;
            DoorsIn = doorsIn;
            DoorsOut = doorsOut;
            DeviceId = deviceId;
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
            List<(int doorId,AntipassBackMode antipassback,int area,DoorDirection direction,DoorType type)> doorsIn,
            List<(int doorId,AntipassBackMode antipassback,int area,DoorDirection direction,DoorType type)> doorsOut
            ) : base(guid)
      {
            Name = name;
            MultiOccMode = multiMode;
            AreaMode = mode;
            OccMax = occMax;
            OccDown = occDown;
            OccUp = occUp;
            Metadata = metadata;
            DoorsIn = doorsIn;
            DoorsOut = doorsOut;
      }
}