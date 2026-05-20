using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Model;

namespace Adapter.Aero.Command;

public sealed class TimeCommand(ILogger<TimeCommand> logger) : BaseCommand,ITimeCommand
{
      public CommandResponse HolidayConfiguration(string Mac, short ScpId, short Year, short Month, short Day, short Extend, short TypeMask)
      {
            CC_SCP_HOL c = new CC_SCP_HOL();
            c.lastModified = 0;
            c.nScpID = ScpId;
            c.number = -1;
            c.year = Year;
            c.month = Month;
            c.day = Day;
            c.extend = Extend;
            c.type_mask = TypeMask;
            var command = LogMessageHelper.ToString(c);
            var result = Send((short)enCfgCmnd.enCcScpHoliday, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.HolidayConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.HolidayConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.HolidayConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.HolidayConfiguration,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                       string.Empty,
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );

            }
      }
      
}