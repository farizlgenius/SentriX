
using Adapter.Aero.Command;
using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Messaging;
using SharedKernel.Model;

namespace Adapter.Aero.Command;

public sealed class InputCommand(ILogger<InputCommand> logger) : BaseCommand, IInputCommand
{
      public CommandResponse ConfigureMonitorPointGroup(
            string Mac,
            short ScpId,
            short MpgNumber,
            List<(short Type,short Number)> MpList
      )
      {
            CC_MPG c = new CC_MPG();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.mpg_number = MpgNumber;
            c.nMpCount = (short)MpList.Count();
            int i = 0;
            foreach(var li in MpList)
            {
                  c.nMpList[i] = li.Type;
                  i+=1;
                  c.nMpList[i] = li.Number;
                  i+=1;
            }
            var result = Send((short)enCfgCmnd.enCcMpg, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ConfigureMonitorPointGroup, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ConfigureMonitorPointGroup,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        c.ToString(),
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ConfigureMonitorPointGroup, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ConfigureMonitorPointGroup,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       c.ToString(),
                       CommandStatus.PENDING.ToString(),
                       string.Empty,
                       false
                       );

            }
      }

      public CommandResponse InputPointSpecification(
            string Mac,
            short ScpId,
            short SioNumber,
            short InputNumber,
            short IcvtNumber,
            short Debounce,
            short HoldTime
      )
      {
            CC_IP c = new CC_IP();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.sio_number = SioNumber;
            c.input = InputNumber;
            c.icvt_num = IcvtNumber;
            c.debounce = Debounce;
            c.hold_time = HoldTime;
            var result = Send((short)enCfgCmnd.enCcInput, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.InputPointSpecification, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.InputPointSpecification,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        c.ToString(),
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.InputPointSpecification, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.InputPointSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       c.ToString(),
                       CommandStatus.PENDING.ToString(),
                       string.Empty,
                       false
                       );

            }
      }

      public CommandResponse MonitorPointConfiguration(
            string Mac,
            short ScpId,
            short MpNumber,
            short SioNumber,
            short IpNo,
            short LfCode,
            short Mode,
            short DelayEntry,
            short DelayExit
      )
      {
            CC_MP c = new CC_MP();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.mp_number = MpNumber;
            c.sio_number = SioNumber;
            c.ip_number = IpNo;
            c.lf_code = LfCode;
            c.mode = Mode;
            c.delay_entry = DelayEntry;
            c.delay_exit = DelayExit;
            var result = Send((short)enCfgCmnd.enCcMP, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.MonitorPointConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.MonitorPointConfiguration,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        c.ToString(),
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.MonitorPointConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.MonitorPointConfiguration,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       c.ToString(),
                       CommandStatus.PENDING.ToString(),
                       string.Empty,
                       false
                       );

            }
      }

      public CommandResponse MonitorPointMask(
            string Mac,
            short ScpId,
            short MpNumber,
            bool IsMask
      )
      {
            CC_MPMASK c = new CC_MPMASK();
            c.scp_number = ScpId;
            c.mp_number = MpNumber;
            c.set_clear = (short)(IsMask ? 1 : 0);
            var result = Send((short)enCfgCmnd.enCcMpMask, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.MonitorPointMask, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.MonitorPointMask,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        c.ToString(),
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.MonitorPointMask, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.MonitorPointMask,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       c.ToString(),
                       CommandStatus.PENDING.ToString(),
                       string.Empty,
                       false
                       );

            }
      }
}
