using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Model;

namespace Adapter.Aero.Command;

public sealed class SettingCommand(ILogger<SettingCommand> logger) : BaseCommand,ISettingCommand
{
      public CommandResponse CardFormatterConfiguration(string Mac, short ScpId, short ComponentId, short Fac, short Offset, short FunctionId, short Flags, short Bits, short PeLn, short PeLoc, short PoLn, short PoLoc, short FcLn, short FcLoc, short ChLn, short ChLoc, short IcLn, short IcLoc)
      {
            CC_SCP_CFMT c = new CC_SCP_CFMT();
            c.lastModified = 0;
            c.nScpID = ScpId;
            c.number = ComponentId;
            c.facility = Fac;
            c.offset = Offset;
            c.function_id = FunctionId;
            c.arg.sensor.flags = Flags;
            c.arg.sensor.bits = Bits;
            c.arg.sensor.pe_ln = PeLn;
            c.arg.sensor.pe_loc = PeLoc;
            c.arg.sensor.po_ln = PoLn;
            c.arg.sensor.po_loc = PoLoc;
            c.arg.sensor.fc_ln = FcLn;
            c.arg.sensor.fc_loc = FcLoc;
            c.arg.sensor.ch_ln = ChLn;
            c.arg.sensor.ch_loc = ChLoc;
            c.arg.sensor.ic_ln = IcLn;
            c.arg.sensor.ic_loc = IcLoc;
            var result = Send((short)enCfgCmnd.enCcScpCfmt, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.CardFormatterConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ExtendedTimeZoneActSpecification,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.CardFormatterConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ExtendedTimeZoneActSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                        c.ToString(),
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );


            }
      }
}