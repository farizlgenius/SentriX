using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class InputRepository(
      IBaseRepository repo,
      ILogger<InputRepository> logger
) : IInputRepository
{
      public CommandResponse InputPointSpecification(string mac, short scpId, short sioNo, short inputNo, short icvtNum, short debounce, short holdTime)
      {
            CC_IP c = new CC_IP();
            c.lastModified = 0;
            c.scp_number = scpId;
            c.sio_number = sioNo;
            c.input = inputNo;
            c.icvt_num = icvtNum;
            c.debounce = debounce;
            c.hold_time = holdTime;
            var result = repo.Send((short)enCfgCmnd.enCcInput, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.InputPointSpecification, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.InputPointSpecification,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.InputPointSpecification, scpId));
                  return new CommandResponse(
                        mac,
                        scpId,
                       Command.InputPointSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                        ObjectHelper.ToAsciiString(c),
                       CommandStatus.PENDING,
                       string.Empty,
                       Vendor.aero,
                       false
                       );

            }
      }

      public CommandResponse MonitorPointConfiguration(string mac, short scpId, short mpNo, short mpSio, short inputNo, short lfCode, short mode, short delayEntry, short delayExit)
      {
            CC_MP c = new CC_MP();
            c.lastModified = 0;
            c.scp_number = scpId;
            c.mp_number = mpNo;
            c.sio_number = mpSio;
            c.ip_number = inputNo;
            c.lf_code = lfCode;
            c.mode = mode;
            c.delay_entry = delayEntry;
            c.delay_exit = delayExit;
            var result = repo.Send((short)enCfgCmnd.enCcMP, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.MonitorPointConfiguration, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.MonitorPointConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.MonitorPointConfiguration, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.MonitorPointConfiguration,
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