using Aero.Application.Constants;
using Aero.Application.Enums;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Area;
using Aero.Application.Metadata.Device;
using Aero.Application.Metadata.Door;
using Aero.Application.Metadata.Procedure;
using Aero.Application.Metadata.User;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class ProcedureRepository(IBaseRepository repo,ILogger<ProcedureRepository> logger) : IProcedureRepository
{
      public CommandResponse ActionSpecification(
            string mac, 
            short scpId, 
            short procId,
            ActionType type,
            short? externalId,
            string? metadata
            )
      {
            CC_ACTN c = new CC_ACTN((short)type);
            c.hdr.lastModified = 0;
            c.hdr.scp_number = scpId;
            c.hdr.proc_number = procId;
            c.hdr.action_type = (short)type;
            switch (type)
            {

                  case ActionType.MonitorPointMask:
                  var maskMeta = JsonHelper.Deserialize<InputMaskMetadata>(metadata ?? "");
                  c.mp_mask.scp_number = scpId;
                  c.mp_mask.mp_number = externalId ?? -1;
                  c.mp_mask.set_clear = (short)maskMeta.SetClear;
                  break;
                  case ActionType.ControlPointCommand:
                  var outMeta = JsonHelper.Deserialize<OutputCommandMetadata>(metadata ?? "");
                  c.cp_ctl.scp_number = scpId;
                  c.cp_ctl.cp_number = externalId ?? -1;
                  c.cp_ctl.command = outMeta.Command;
                  c.cp_ctl.on_time = outMeta.OnTime;
                  c.cp_ctl.off_time = outMeta.OffTime;
                  c.cp_ctl.repeat = outMeta.Repeat;
                  break;
                  case ActionType.DoorMode:
                  case ActionType.ForceOpenMask:
                  case ActionType.HeldOpenMask:
                  case ActionType.MomentaryUnlock:
                  case ActionType.ReaderLEDModeControl:
                  case ActionType.AntipassbackControlCommand:
                  case ActionType.AbortWaitForDoorOpenState:
                  case ActionType.TempReaderLEDControl:
                  case ActionType.TextLCDTerminal:
                  case ActionType.HostSimulateCardRead:
                  //externalId = dto.SrcGuid == null ? null : await door.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.ProcedureControlCommand:
                  //externalId = dto.SrcGuid == null ? null : await procedure.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.TriggerVariableControlCommand:
                   //externalId = dto.SrcGuid == null ? null : await triggerVariable.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.TimeZoneControl:
                  //externalId = dto.SrcGuid == null ? null : await time.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.MonitorPointGroupArmDisarmCommand:
                  //externalId = dto.SrcGuid == null ? null : await monitorGroup.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.ModifyAreaConfiguration:
                  //externalId = dto.SrcGuid == null ? null : await area.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.SetUseLimit:
                  case ActionType.SetOperationMode:
                  case ActionType.HostSimulatedKeyPress:
                  case ActionType.FilterOutTriggerActivationSummary:
                  case ActionType.TriggerActivationSummary:
                  case ActionType.DelayMillisecIncrement:
                  case ActionType.DelaySecIncrement:
                  var delayMeta = JsonHelper.Deserialize<DelayMetadata>(metadata ?? "");
                  c.delay.delay_time = (short)delayMeta.DelayTime;
                  break;
                  case ActionType.DeleteAll:
                  default:
                  break;
            }

            var result = repo.Send((short)enCfgCmnd.enCcProc, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ActionSpecification, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.ActionSpecification,
                        SCPDLL.scpGetTagLastPosted(scpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                       ObjectHelper.ToAsciiString(c),
                        CommandStatus.PENDING,
                        string.Empty,
                        Vendor.aero,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ActionSpecification, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.ActionSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       ObjectHelper.ToAsciiString(c),
                       CommandStatus.FAILED,
                       string.Empty,
                       Vendor.aero,
                       false
                       );

            }
      }
      // public CommandResponse ActionSpecification(string mac, short scpId, short procId,Dictionary<ActionType,string> values)
      // {
      //       CC_ACTN  c = new();

      //       // hdr
      //       c.hdr = new CC_ACTN_HDR
      //             {
      //                   lastModified=0,
      //                   scp_number = scpId,
      //                   proc_number=procId
      //             };

      //       // // mp_mask
      //       // if (JsonHelper.Deserialize<InputMaskMetadata>(values[ActionType.MonitorPointMask]) is var maskMeta && maskMeta is not null)
      //       // {
      //       //       c.mp_mask = new CC_MPMASK
      //       //       {
      //       //             scp_number = scpId,
      //       //             mp_number = maskMeta.InputNumber,    // Example property name
      //       //             set_clear = (short)maskMeta.SetClear     // Example property name
      //       //       };
      //       // }

      //       // // cp control
      //       // if (JsonHelper.Deserialize<OutputCommandMetadata>(values[ActionType.ControlPointCommand]) is var outMeta && outMeta is not null)
      //       // {
      //       //       c.cp_ctl = new CC_CPCTL
      //       //       {
      //       //             scp_number = scpId,
      //       //             cp_number = outMeta.OutputNumber,    
      //       //             command = outMeta.Command,
      //       //             on_time = outMeta.OnTime,
      //       //             off_time = outMeta.OffTime,
      //       //             repeat = outMeta.Repeat     
      //       //       }; 
      //       // }

      //       // acr mode
      //       if (JsonHelper.Deserialize<DoorModeMetadata>(values[ActionType.ControlPointCommand]) is var doorModeMeta && doorModeMeta is not null)
      //       {
      //             c.acr_mode = new CC_ACRMODE
      //             {
      //                   scp_number = scpId,
      //                   acr_number = doorModeMeta.DoorNumber,    
      //                   acr_mode = (short)doorModeMeta.Mode
      //             };
      //       }

      //       // fo_mask
      //       if (JsonHelper.Deserialize<ForceOpenMaskMetadata>(values[ActionType.ForceOpenMask]) is var forceOpenMaskMeta && forceOpenMaskMeta is not null)
      //       {
      //             c.fo_mask = new CC_FOMASK
      //             {
      //                   scp_number = scpId,
      //                   acr_number = forceOpenMaskMeta.DoorNumber,    
      //                   set_clear = (short)forceOpenMaskMeta.SetClear
      //             };
      //       }

      //       // ho_maks
      //       if (JsonHelper.Deserialize<ForceOpenMaskMetadata>(values[ActionType.HeldOpenMask]) is var heldOpenMaskMeta && heldOpenMaskMeta is not null)
      //       {
      //             c.ho_mask = new CC_HOMASK {
      //                   scp_number = scpId,
      //                   acr_number = heldOpenMaskMeta.DoorNumber,
      //                   set_clear = (short)heldOpenMaskMeta.SetClear
      //             };
      //       }

      //       // unlock
      //       if (JsonHelper.Deserialize<MomentaryUnlockMetadata>(values[ActionType.MomentaryUnlock]) is var unlockMeta && unlockMeta is not null)
      //       {
      //             c.unlock = new CC_UNLOCK {
      //                   scp_number = scpId,
      //                   acr_number = unlockMeta.DoorNumber,
      //                   floor_number = unlockMeta.FloorNumber,
      //                   strk_tm = unlockMeta.StrikeTime,
      //                   t_held = unlockMeta.HeldOpenTime,
      //                   t_held_pre = unlockMeta.HeldOpenPreAlarm,
      //             };
      //       }

      //       // proc
      //       if (JsonHelper.Deserialize<ProcedureCommandMetadata>(values[ActionType.ProcedureControlCommand]) is var procMeta && procMeta is not null)
      //       {
      //             c.proc = new CC_PROCEDURE {
      //                   scp_number = scpId,
      //                   proc_number = procMeta.ProcedureNumber,
      //                   command = (short)procMeta.Command
      //             };
      //       }

      //       // tv_ctl
      //       if (JsonHelper.Deserialize<TriggerVariableControlCommandMetadata>(values[ActionType.TriggerVariableControlCommand]) is var tvMeta && tvMeta is not null)
      //       {
      //             c.tv_ctl = new CC_TVCOMMAND {
      //                   scp_number = scpId,
      //                   tv_number = tvMeta.TriggerNumber,
      //                   set_clear = tvMeta.SetClear
      //             };
      //       }

      //       // tz_ctl
      //       if (JsonHelper.Deserialize<TimeZoneCommandMetadata>(values[ActionType.TimeZoneControl]) is var tzMeta && tzMeta is not null)
      //       {
      //             c.tz_ctl = new CC_TZCOMMAND
      //             {
      //                   scp_number = scpId,
      //                   tz_number = tzMeta.TimeZoneNumber,
      //                   command = (short)tzMeta.Command

      //             };
      //       }

      //       // led_mode
      //       if (JsonHelper.Deserialize<ReaderLEDMetadata>(values[ActionType.ReaderLEDModeControl]) is var ledMeta && ledMeta is not null)
      //       {
      //             c.led_mode = new CC_ACRLEDMODE
      //             {
      //                   scp_number = scpId,
      //                   acr_number = ledMeta.DoorNumber,
      //                   led_mode = (short)ledMeta.Mode

      //             };
      //       }

      //       // mpg
      //       if (JsonHelper.Deserialize<MonitorPointGroupArmDisarmMetadata>(values[ActionType.MonitorPointGroupArmDisarmCommand]) is var mpgMeta && mpgMeta is not null)
      //       {
      //             c.mpg_set = new CC_MPGSET
      //             {
      //                   scp_number = scpId,
      //                   mpg_number = mpgMeta.InputGroupNumber,
      //                   command = (short)mpgMeta.Command,
      //                   arg1=mpgMeta.Arg1

      //             };
      //       }

      //       // area
      //       if (JsonHelper.Deserialize<AreaConfigurationMetadata>(values[ActionType.ModifyAreaConfiguration]) is var areaMeta && areaMeta is not null)
      //       {
      //             c.area_set = new CC_AREASET
      //             {
      //                   scp_number = scpId,
      //                   area_number = areaMeta.AreaNumber,
      //                   command = (short)areaMeta.Command,
      //                   occ_set=areaMeta.OccSet

      //             };
      //       }

      //       // temp acr
      //       if (JsonHelper.Deserialize<TempDoorModeMetadata>(values[ActionType.TempDoorMode]) is var tempDoorMeta && tempDoorMeta is not null)
      //       {
      //             c.temp_acr_mode = new CC_TEMP_ACR_MODE
      //             {
      //                   scp_number = scpId,
      //                   acr_number = tempDoorMeta.DoorNumber,
      //                   acr_mode = (short)tempDoorMeta.Mode,
      //                   time = tempDoorMeta.Time

      //             };
      //       }

      //       // use limit
      //       if (JsonHelper.Deserialize<UseLimitMetadata>(values[ActionType.SetUseLimit]) is var limitMeta && limitMeta is not null)
      //       {
      //             c.use_limit = new CC_USELIMITI64
      //             {
      //                   scp_number = scpId,
      //                   cardholder_id = limitMeta.CardNo,
      //                   new_limit = limitMeta.NewLimit,

      //             };
      //       }

      //       // Delay
      //       if (values.TryGetValue(ActionType.DelayMinIncrement, out _))
      //       {
      //             c.delay = new CC_DELAY
      //             {
      //                   delay_time = short.Parse(values[ActionType.DelayMinIncrement])
      //             };
      //       }



      //       var result = repo.Send((short)enCfgCmnd.enCcProc, c);
      //       if (result)
      //       {
      //             logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ActionSpecification, scpId));

      //             return new CommandResponse(
      //                   mac,
      //                   scpId,
      //                   Command.ActionSpecification,
      //                   SCPDLL.scpGetTagLastPosted(scpId),
      //                   DateTime.UtcNow,
      //                   DateTime.UtcNow,
      //                  ObjectHelper.ToAsciiString(c),
      //                   CommandStatus.PENDING,
      //                   string.Empty,
      //                   Vendor.aero,
      //                   true
      //                   );

      //       }
      //       else
      //       {
      //             logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ActionSpecification, scpId));
      //             return new CommandResponse(
      //                   mac,
      //                  scpId,
      //                  Command.ActionSpecification,
      //                  -1,
      //                  DateTime.UtcNow,
      //                  DateTime.UtcNow,
      //                  ObjectHelper.ToAsciiString(c),
      //                  CommandStatus.FAILED,
      //                  string.Empty,
      //                  Vendor.aero,
      //                  false
      //                  );

      //       }
            
      // }
      // public CommandResponse ActionSpecification(string mac, short scpId, short procId,Dictionary<ActionType,object> values)
      // {
      //       CC_ACTN c = new CC_ACTN
      //       {
                  
      //             hdr = new CC_ACTN_HDR
      //             {
      //                   lastModified=0,
      //                   scp_number = scpId,
      //                   proc_number=procId
      //             },

      //             // mp_mask = values.TryGetValue(ActionType.MonitorPointMask, out var obj1) && obj1 is InputMaskMetadata maskMeta
      //             // ? new CC_MPMASK
      //             // {
      //             //       scp_number = scpId,
      //             //       mp_number = maskMeta.InputNumber,    // Example property name
      //             //       set_clear = (short)maskMeta.SetClear     // Example property name
      //             // } 
      //             // : null,

      //             // cp_ctl = values.TryGetValue(ActionType.ControlPointCommand, out var obj2) && obj2 is OutputCommandMetadata outMeta
      //             // ? new CC_CPCTL
      //             // {
      //             //       scp_number = scpId,
      //             //       cp_number = outMeta.OutputNumber,    
      //             //       command = outMeta.Command,
      //             //       on_time = outMeta.OnTime,
      //             //       off_time = outMeta.OffTime,
      //             //       repeat = outMeta.Repeat     
      //             // } 
      //             // : null,

      //             acr_mode = values.TryGetValue(ActionType.DoorMode, out var obj3) && obj3 is DoorModeMetadata doorModeMeta
      //             ? new CC_ACRMODE
      //             {
      //                   scp_number = scpId,
      //                   acr_number = doorModeMeta.DoorNumber,    
      //                   acr_mode = (short)doorModeMeta.Mode
      //             } 
      //             : null,

      //             fo_mask = values.TryGetValue(ActionType.ForceOpenMask, out var obj4) && obj4 is ForceOpenMaskMetadata forceOpenMaskMeta
      //             ? new CC_FOMASK
      //             {
      //                   scp_number = scpId,
      //                   acr_number = forceOpenMaskMeta.DoorNumber,    
      //                   set_clear = (short)forceOpenMaskMeta.SetClear
      //             } 
      //             : null,

      //             ho_mask = values.TryGetValue(ActionType.HeldOpenMask,out var obj5) && obj5 is HeldOpenMaskMetadata heldOpenMaskMeta
      //             ? new CC_HOMASK {
      //                   scp_number = scpId,
      //                   acr_number = heldOpenMaskMeta.DoorNumber,
      //                   set_clear = (short)heldOpenMaskMeta.SetClear
      //             }
      //             : null,

      //             unlock = values.TryGetValue(ActionType.MomentaryUnlock,out var obj6) && obj6 is MomentaryUnlockMetadata unlockMeta
      //             ? new CC_UNLOCK {
      //                   scp_number = scpId,
      //                   acr_number = unlockMeta.DoorNumber,
      //                   floor_number = unlockMeta.FloorNumber,
      //                   strk_tm = unlockMeta.StrikeTime,
      //                   t_held = unlockMeta.HeldOpenTime,
      //                   t_held_pre = unlockMeta.HeldOpenPreAlarm,
      //             }
      //             : null,

      //             proc = values.TryGetValue(ActionType.ProcedureControlCommand,out var obj7) && obj7 is ProcedureCommandMetadata procMeta
      //             ? new CC_PROCEDURE {
      //                   scp_number = scpId,
      //                   proc_number = procMeta.ProcedureNumber,
      //                   command = (short)procMeta.Command
      //             }
      //             : null,

      //             tv_ctl = values.TryGetValue(ActionType.TriggerVariableControlCommand,out var obj8) && obj8 is TriggerVariableControlCommandMetadata tvMeta
      //             ? new CC_TVCOMMAND {
      //                   scp_number = scpId,
      //                   tv_number = tvMeta.TriggerNumber,
      //                   set_clear = tvMeta.SetClear
      //             }
      //             : null,

      //             tz_ctl = values.TryGetValue(ActionType.TimeZoneControl,out var obj9) && obj9 is TimeZoneCommandMetadata tzMeta 
      //             ? new CC_TZCOMMAND
      //             {
      //                   scp_number = scpId,
      //                   tz_number = tzMeta.TimeZoneNumber,
      //                   command = (short)tzMeta.Command

      //             }
      //             : null,

      //             led_mode = values.TryGetValue(ActionType.ReaderLEDModeControl,out var obj10) && obj10 is ReaderLEDMetadata ledMeta 
      //             ? new CC_ACRLEDMODE
      //             {
      //                   scp_number = scpId,
      //                   acr_number = ledMeta.DoorNumber,
      //                   led_mode = (short)ledMeta.Mode

      //             }
      //             : null,

      //             mpg_set = values.TryGetValue(ActionType.MonitorPointGroupArmDisarmCommand,out var obj11) && obj11 is MonitorPointGroupArmDisarmMetadata mpgMeta 
      //             ? new CC_MPGSET
      //             {
      //                   scp_number = scpId,
      //                   mpg_number = mpgMeta.InputGroupNumber,
      //                   command = (short)mpgMeta.Command,
      //                   arg1=mpgMeta.Arg1

      //             }
      //             : null,

      //             area_set = values.TryGetValue(ActionType.ModifyAreaConfiguration,out var obj12) && obj12 is AreaConfigurationMetadata areaMeta 
      //             ? new CC_AREASET
      //             {
      //                   scp_number = scpId,
      //                   area_number = areaMeta.AreaNumber,
      //                   command = (short)areaMeta.Command,
      //                   occ_set=areaMeta.OccSet

      //             }
      //             : null,

      //             temp_acr_mode = values.TryGetValue(ActionType.TempDoorMode,out var obj13) && obj13 is TempDoorModeMetadata tempDoorMeta 
      //             ? new CC_TEMP_ACR_MODE
      //             {
      //                   scp_number = scpId,
      //                   acr_number = tempDoorMeta.DoorNumber,
      //                   acr_mode = (short)tempDoorMeta.Mode,
      //                   time = tempDoorMeta.Time

      //             }
      //             : null,

      //             use_limit = values.TryGetValue(ActionType.SetUseLimit,out var obj14) && obj14 is UseLimitMetadata limitMeta 
      //             ? new CC_USELIMITI64
      //             {
      //                   scp_number = scpId,
      //                   cardholder_id = limitMeta.CardNo,
      //                   new_limit = limitMeta.NewLimit,

      //             }
      //             : null,

      //             delay = values.TryGetValue(ActionType.DelayMinIncrement,out var obj15) && obj15 is short time 
      //             ? new CC_DELAY
      //             {
      //                   delay_time = time
      //             }
      //             : null,
      //       };

      //       var result = repo.Send((short)enCfgCmnd.enCcProc, c);
      //       if (result)
      //       {
      //             logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ActionSpecification, scpId));

      //             return new CommandResponse(
      //                   mac,
      //                   scpId,
      //                   Command.ActionSpecification,
      //                   SCPDLL.scpGetTagLastPosted(scpId),
      //                   DateTime.UtcNow,
      //                   DateTime.UtcNow,
      //                  ObjectHelper.ToAsciiString(c),
      //                   CommandStatus.PENDING,
      //                   string.Empty,
      //                   Vendor.aero,
      //                   true
      //                   );

      //       }
      //       else
      //       {
      //             logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ActionSpecification, scpId));
      //             return new CommandResponse(
      //                   mac,
      //                  scpId,
      //                  Command.ActionSpecification,
      //                  -1,
      //                  DateTime.UtcNow,
      //                  DateTime.UtcNow,
      //                  ObjectHelper.ToAsciiString(c),
      //                  CommandStatus.FAILED,
      //                  string.Empty,
      //                  Vendor.aero,
      //                  false
      //                  );

      //       }
            
      // }
}