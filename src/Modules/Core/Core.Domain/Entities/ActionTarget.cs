using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class ActionTarget 
{
      public int? DoorId {get; private set; }
      public int? MonitorId {get; private set; }
      public int? ControlId {get; private set; }
      public int? AreaId {get; private set; }
      public int? UserId {get; private set; }
      public int? TriggerVariableId {get; private set; }
      public int? ProcedureId {get; private set; }
      public int? TimeZoneId {get; private set; }
      public int? MonitorGroupId {get; private set; }
      public int? TriggerId {get; private set; }
      // Relation
      public short? ExternalId {get; private set; }
      public ActionTarget(
            ActionType type,
            int targetId,
            short? externalId=null
      )
      {
            switch (type)
            {
                  case ActionType.MonitorPointMask:
                  MonitorId = targetId;
                  break;
                  case ActionType.ControlPointCommand:
                  ControlId = targetId;
                  break;
                  case ActionType.DoorMode:
                  case ActionType.ForceOpenMask:
                  case ActionType.HeldOpenMask:
                  case ActionType.MomentaryUnlock:
                  case ActionType.ReaderLEDModeControl:
                  case ActionType.AbortWaitForDoorOpenState:
                  case ActionType.TempReaderLEDControl:
                  case ActionType.TempDoorMode:
                  case ActionType.HostSimulateCardRead:
                  case ActionType.HostSimulatedKeyPress:
                  DoorId = targetId;
                  break;
                  case ActionType.ProcedureControlCommand:
                  ProcedureId = targetId;
                  break;
                  case ActionType.TriggerVariableControlCommand:
                  TriggerVariableId = targetId;
                  break;
                  case ActionType.TimeZoneControl:
                  TimeZoneId = targetId;
                  break;
                  case ActionType.AntipassbackControlCommand:
                  case ActionType.SetUseLimit:
                  UserId = targetId;
                  break;
                  case ActionType.MonitorPointGroupArmDisarmCommand:
                  MonitorGroupId = targetId;
                  break;
                  case ActionType.ModifyAreaConfiguration:
                  AreaId = targetId;
                  break;
                  case ActionType.TriggerActivationSummary:
                  case ActionType.FilterOutTriggerActivationSummary:
                  TriggerId = targetId;
                  break;
                  case ActionType.DelayMinIncrement:
                  case ActionType.DelayMillisecIncrement:
                  case ActionType.DeleteAll:
                  default:
                  break;
            }       
            ExternalId = externalId;  
      }
}