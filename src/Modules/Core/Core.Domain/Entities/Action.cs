using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Action : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public ActionType Type {get; private set;}
      public string? Metadata {get; private set;}
      public IEnumerable<ActionTarget> ActionTarget {get; private set; } = default!;
      public short Sequence {get; private set; }
      public int DeviceId {get; private set;}
      public int LocationId {get; private set;}
      public int ProcedureId {get; private set; }

      public Action(
            string name,
            ActionType type,
            int deviceId,
            int locationId,
            short sequence,
            IEnumerable<(int id,short externalId)> actionTarget,
            string? metadata=null,
            short? externalId=null
      ) : base(Guid.NewGuid(),externalId)
      {
            Name = name;
            Type = type;
            Metadata = metadata;
            DeviceId = deviceId;
            LocationId = locationId;
            Sequence = sequence;
            ActionTarget = [.. actionTarget.Select(x => new Entities.ActionTarget(type,x.id,x.externalId))];

      }

      public Action(
            Guid guid,
           string name,
            ActionType type,
            int deviceId,
            int locationId,
            short sequence,
            IEnumerable<(int id,short externalId)> actionTarget,
            string? metadata=null,
            short? externalId=null
      ) : base(guid,externalId)
      {
            Name = name;
            Type = type;
            Metadata = metadata;
            DeviceId = deviceId;
            LocationId = locationId;
            Sequence = sequence;
            ActionTarget = [.. actionTarget.Select(x => new Entities.ActionTarget(type,x.id,x.externalId))];
      }     
}