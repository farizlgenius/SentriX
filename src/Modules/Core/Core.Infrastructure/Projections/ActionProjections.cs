using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Action;
using SharedKernel.Enums;

namespace Core.Infrastructure.Projections;

public static class ActionProjections
{
    public static readonly Expression<Func<Persistences.Entities.Action, ActionDto>> ToDto =
    x => new ActionDto(
          x.guid,
          x.name,
          x.device.guid,
          x.device.name,
          x.type,
          x.type.ToString(),
          x.sequence,
          null,
          null,
          x.metadata,
          x.is_active,
          x.is_default
    );

    public static readonly Expression<Func<Persistences.Entities.Action, CommandDetailDto>> ToCommand =
  x => new CommandDetailDto(
      x.device.id,
      x.device.external_id ?? -1,
      x.device.mac,
      x.device.ip,
      -1,
      -1,
      x.device.vendor,
      x.location_id,
      x.id,
      x.external_id
  );

    private static ActionTargetDto? SwitcherTarget(ActionType type, Persistences.Entities.ActionTarget target)
    {
        return type switch
        {
            ActionType.MonitorPointMask => target.monitor == null ? null : new ActionTargetDto(target.monitor.guid, target.monitor.name),
            ActionType.ControlPointCommand => target.control == null ? null : new ActionTargetDto(target.control.guid, target.control.name),
            ActionType.DoorMode => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.ForceOpenMask => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.HeldOpenMask => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.MomentaryUnlock => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.ReaderLEDModeControl => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.AbortWaitForDoorOpenState => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.TempReaderLEDControl => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.TempDoorMode => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.HostSimulateCardRead => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.HostSimulatedKeyPress => target.door == null ? null : new ActionTargetDto(target.door.guid, target.door.name),
            ActionType.ProcedureControlCommand => target.procedure == null ? null : new ActionTargetDto(target.procedure.guid, target.procedure.name),
            ActionType.TriggerVariableControlCommand => target.trigger_variable == null ? null : new ActionTargetDto(target.trigger_variable.guid, target.trigger_variable.name),
            ActionType.TimeZoneControl => target.timezone == null ? null : new ActionTargetDto(target.timezone.guid, target.timezone.name),
            ActionType.AntipassbackControlCommand => target.user == null ? null : new ActionTargetDto(target.user.guid, target.user.name),
            ActionType.SetUseLimit => target.user == null ? null : new ActionTargetDto(target.user.guid, target.user.name),
            ActionType.MonitorPointGroupArmDisarmCommand => target.monitor_group == null ? null : new ActionTargetDto(target.monitor_group.guid, target.monitor_group.name),
            ActionType.ModifyAreaConfiguration => target.area == null ? null : new ActionTargetDto(target.area.guid, target.area.name),
            ActionType.TriggerActivationSummary => target.trigger == null ? null : new ActionTargetDto(target.trigger.guid, target.trigger.name),
            ActionType.FilterOutTriggerActivationSummary => target.trigger == null ? null : new ActionTargetDto(target.trigger.guid, target.trigger.name),
            ActionType.DelaySecIncrement => null,
            ActionType.DelayMillisecIncrement => null,
            ActionType.DeleteAll => null,
            _ => null
        };
    }
}