using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Aero.Command;

public sealed class TimeCommand(ILogger<TimeCommand> logger) : BaseCommand,ITimeCommand
{
      public CommandResponse ExtendedTimezoneActSpecification(
            string Mac,
            short ScpId,
            short TzNumber,
            short Mode,
            string Active,
            string Deactive,
            List<IntervalDto> intervals

      )
      {
            CC_SCP_TZEX_ACT c= new CC_SCP_TZEX_ACT();
            c.lastModified = 0;
            c.nScpID = ScpId;
            c.number = TzNumber;
            c.mode = Mode;
            c.actTime = (int)UtilitiesHelper.DateTimeToElapeSecond(Active);
            c.deactTime = (int)UtilitiesHelper.DateTimeToElapeSecond(Deactive);
            c.intervals = (short)intervals.Count;
            int i = 0;
            foreach(var interval in intervals)
            {
                  c.i[i].i_days = (short)UtilitiesHelper.ConvertDayToBinary(
                        interval.Days.Sunday,
                        interval.Days.Monday,
                        interval.Days.Tuesday,
                        interval.Days.Wednesday,
                        interval.Days.Thursday,
                        interval.Days.Friday,
                        interval.Days.Saturday);

                  c.i[i].i_start = (short)UtilitiesHelper.ConvertTimeToEndMinute(interval.Start);
                  c.i[i].i_end = (short)UtilitiesHelper.ConvertTimeToEndMinute(interval.End);
                  i++;
            }
            var result = Send((short)enCfgCmnd.enCcScpTimezoneExAct, c);
            if (false)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ExtendedTimeZoneActSpecification, ScpId));

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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ExtendedTimeZoneActSpecification, ScpId));
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