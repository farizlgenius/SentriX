using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class TimeRepository(
      IBaseRepository repo,
      ILogger<TimeRepository> logger
) : ITimeRepository
{
      public CommandResponse ExtendedTimeZoneActSpecification(string mac, short scpId, short tzNumber, short mode,List<(short i_day,short start,short end)> intervalDetail)
      {
            CC_SCP_TZEX_ACT c = new CC_SCP_TZEX_ACT();
            c.lastModified = 0;
            c.nScpID = scpId;
            c.number = tzNumber;
            c.mode = mode;
            c.actTime = 0;
            c.deactTime = 0;
            c.intervals = (short)intervalDetail.Count();
            int i = 0;
            foreach(var inter in intervalDetail)
            {
                  c.i[i].i_days = inter.i_day;
                  c.i[i].i_start = inter.start;
                  c.i[i].i_end = inter.end;
                  i++;
            }
            var result = repo.Send((short)enCfgCmnd.enCcScpTimezoneExAct, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ExtendedTimeZoneActSpecification, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.ExtendedTimeZoneActSpecification,
                        SCPDLL.scpGetTagLastPosted(scpId),
                        DateTime.UtcNow,
                        null,
                       ObjectHelper.ToAsciiString(c),
                        CommandStatus.PENDING,
                        string.Empty,
                        Vendor.aero,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ExtendedTimeZoneActSpecification, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.ExtendedTimeZoneActSpecification,
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