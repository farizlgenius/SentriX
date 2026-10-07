using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class GroupRepository(
      IBaseRepository repo,
      ILogger<GroupRepository> logger
) : IGroupRepository
{
      public CommandResponse AddAccessGroup(string mac, short scpId, short groupId, short operMode, List<(short doorId, short timeZoneId)> datas)
      {
           CC_ALVL_EX c = new CC_ALVL_EX();
           c.lastModified = 0;
           c.scp_number = scpId;
           c.alvl_number = groupId;
           c.oper_mode = operMode;
           foreach(var d in datas)
            {
                  c.tz[d.doorId] = d.timeZoneId;
            }
            var result = repo.Send((short)enCfgCmnd.enCcAlvlEx, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.AccessLevelConfigurationExtended, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.AccessLevelConfigurationExtended,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.AccessLevelConfigurationExtended, scpId));
                  return new CommandResponse(
                        mac,
                        scpId,
                       Command.AccessLevelConfigurationExtended,
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
}