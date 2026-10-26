using Aero.Application.Constants;
using Aero.Application.Enums;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Area;
using Aero.Application.Metadata.Device;
using Aero.Application.Metadata.Door;
using Aero.Application.Metadata.User;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class ProcedureRepository(IBaseRepository repo,ILogger<ProcedureRepository> logger) : IProcedureRepository
{
      // public CommandResponse ActionSpecification(string mac,short scpId,short procId,ActionType type,object values)
      // {
      //       CC_ACTN c = new CC_ACTN((short)type);
      //       c.hdr.lastModified = 0;
      //       c.hdr.scp_number = scpId;
      //       c.hdr.proc_number = procId;
      //       switch (type)
      //       {
      //             case ActionType.MonitorPointMask:
      //             if(values is not InputMaskMetadata)
      //                   break;
      //             c.mp_mask.scp_number = scpId;
      //             c.mp_mask.mp_number = values.
      //             break;
      //             default:
      //             break;
      //       }
      // }

      public CommandResponse ActionSpecification(string mac, short scpId, short procId,Dictionary<ActionType,object> values)
      {
            CC_ACTN c = new CC_ACTN
            {
                  
                  hdr = new CC_ACTN_HDR
                  {
                        lastModified=0,
                        scp_number = scpId,
                        proc_number=procId
                  },

                  mp_mask = values.TryGetValue(ActionType.MonitorPointMask, out var obj1) && obj1 is InputMaskMetadata maskMeta
                  ? new CC_MPMASK
                  {
                        scp_number = scpId,
                        mp_number = maskMeta.InputNumber,    // Example property name
                        set_clear = (short)maskMeta.SetClear     // Example property name
                  } 
                  : null,

                  cp_ctl = values.TryGetValue(ActionType.ControlPointCommand, out var obj2) && obj2 is OutputCommandMetadata outMeta
                  ? new CC_CPCTL
                  {
                        scp_number = scpId,
                        cp_number = outMeta.OutputNumber,    
                        command = outMeta.Command,
                        on_time = outMeta.OnTime,
                        off_time = outMeta.OffTime,
                        repeat = outMeta.Repeat     
                  } 
                  : null,

                  acr_mode = values.TryGetValue(ActionType.DoorMode, out var obj3) && obj3 is DoorModeMetadata doorModeMeta
                  ? new CC_ACRMODE
                  {
                        scp_number = scpId,
                        acr_number = doorModeMeta.DoorNumber,    
                        acr_mode = (short)doorModeMeta.Mode
                  } 
                  : null,

                  fo_mask = values.TryGetValue(ActionType.ForceOpenMask, out var obj4) && obj4 is ForceOpenMaskMetadata forceOpenMaskMeta
                  ? new CC_FOMASK
                  {
                        scp_number = scpId,
                        acr_number = forceOpenMaskMeta.DoorNumber,    
                        set_clear = (short)forceOpenMaskMeta.SetClear
                  } 
                  : null,

                  ho_mask = values.TryGetValue(ActionType.HeldOpenMask,out var obj5) && obj5 is HeldOpenMaskMetadata heldOpenMaskMeta
                  ? new CC_HOMASK {
                        scp_number = scpId,
                        acr_number = heldOpenMaskMeta.DoorNumber,
                        set_clear = (short)heldOpenMaskMeta.SetClear
                  }
                  : null,

                  unlock = values.TryGetValue(ActionType.MomentaryUnlock,out var obj6) && obj6 is MomentaryUnlockMetadata unlockMeta
                  ? new CC_UNLOCK {
                        scp_number = scpId,
                        acr_number = unlockMeta.DoorNumber,
                        floor_number = unlockMeta.FloorNumber,
                        strk_tm = unlockMeta.StrikeTime,
                        t_held = unlockMeta.HeldOpenTime,
                        t_held_pre = unlockMeta.HeldOpenPreAlarm,
                  }
                  : null,

                  proc = values.TryGetValue(ActionType.ProcedureControlCommand,out var obj7) && obj7 is ProcedureCommandMetadata procMeta
                  ? new CC_PROCEDURE {
                        scp_number = scpId,
                        proc_number = procMeta.ProcedureNumber,
                        command = (short)procMeta.Command
                  }
                  : null,

                  tv_ctl = values.TryGetValue(ActionType.TriggerVariableControlCommand,out var obj8) && obj8 is TriggerVariableControlCommandMetadata tvMeta
                  ? new CC_TVCOMMAND {
                        scp_number = scpId,
                        tv_number = tvMeta.TriggerNumber,
                        set_clear = tvMeta.SetClear
                  }
                  : null,

                  tz_ctl = values.TryGetValue(ActionType.TimeZoneControl,out var obj9) && obj9 is TimeZoneCommandMetadata tzMeta 
                  ? new CC_TZCOMMAND
                  {
                        scp_number = scpId,
                        tz_number = tzMeta.TimeZoneNumber,
                        command = (short)tzMeta.Command

                  }
                  : null,

                  led_mode = values.TryGetValue(ActionType.ReaderLEDModeControl,out var obj10) && obj10 is ReaderLEDMetadata ledMeta 
                  ? new CC_ACRLEDMODE
                  {
                        scp_number = scpId,
                        acr_number = ledMeta.DoorNumber,
                        led_mode = (short)ledMeta.Mode

                  }
                  : null,

                  mpg_set = values.TryGetValue(ActionType.MonitorPointGroupArmDisarmCommand,out var obj11) && obj11 is MonitorPointGroupArmDisarmMetadata mpgMeta 
                  ? new CC_MPGSET
                  {
                        scp_number = scpId,
                        mpg_number = mpgMeta.InputGroupNumber,
                        command = (short)mpgMeta.Command,
                        arg1=mpgMeta.Arg1

                  }
                  : null,

                  area_set = values.TryGetValue(ActionType.ModifyAreaConfiguration,out var obj12) && obj12 is AreaConfigurationMetadata areaMeta 
                  ? new CC_AREASET
                  {
                        scp_number = scpId,
                        area_number = areaMeta.AreaNumber,
                        command = (short)areaMeta.Command,
                        occ_set=areaMeta.OccSet

                  }
                  : null,

                  temp_acr_mode = values.TryGetValue(ActionType.TempDoorMode,out var obj13) && obj13 is TempDoorModeMetadata tempDoorMeta 
                  ? new CC_TEMP_ACR_MODE
                  {
                        scp_number = scpId,
                        acr_number = tempDoorMeta.DoorNumber,
                        acr_mode = (short)tempDoorMeta.Mode,
                        time = tempDoorMeta.Time

                  }
                  : null,

                  use_limit = values.TryGetValue(ActionType.SetUseLimit,out var obj14) && obj14 is UseLimitMetadata limitMeta 
                  ? new CC_USELIMITI64
                  {
                        scp_number = scpId,
                        cardholder_id = limitMeta.CardNo,
                        new_limit = limitMeta.NewLimit,

                  }
                  : null,

                  delay = values.TryGetValue(ActionType.DelayMinIncrement,out var obj15) && obj15 is short time 
                  ? new CC_DELAY
                  {
                        delay_time = time
                  }
                  : null,
            };

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
}