
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
                        string.Empty,
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
                       string.Empty,
                       CommandStatus.PENDING.ToString(),
                       string.Empty,
                       false
                       );

            }
      }


}
