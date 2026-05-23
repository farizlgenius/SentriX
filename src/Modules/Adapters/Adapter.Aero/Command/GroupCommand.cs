using System.Security.Cryptography.X509Certificates;
using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Model;

namespace Adapter.Aero.Command;

public sealed class GroupCommand(ILogger<GroupCommand> logger) : BaseCommand, IGroupCommand
{
      public CommandResponse AccessLevelConfigurationExtended(string Mac,short ScpId,short ComponentId,short[] Timezone)
      {
            CC_ALVL_EX c = new CC_ALVL_EX();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.alvl_number = ComponentId;
            c.oper_mode = 1;
            for(int i = 0;i < c.tz.Length; i++)
            {
                  c.tz[i] = Timezone[i];
            }
            var result = Send((short)enCfgCmnd.enCcAlvlEx, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.AccessControlReaderConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.AccessControlReaderConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.AccessControlReaderConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.AccessControlReaderConfiguration,
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