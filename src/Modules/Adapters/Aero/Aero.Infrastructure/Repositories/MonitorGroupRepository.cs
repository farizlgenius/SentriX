using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;


public sealed class MonitorGroupRepository(IBaseRepository repo, ILogger<MonitorGroupRepository> logger) : IMonitorGroupRepository
{
      public CommandResponse ConfigureMonitorPointGroup(string mac, short scpId, short mpgId, short mpgCount, IEnumerable<(MonitorType, short)> mpgList)
      {
            CC_MPG c = new CC_MPG();
            c.lastModified = 0;
            c.scp_number = scpId;
            c.mpg_number = mpgId;
            c.nMpCount = mpgCount;
            int j = 0;
            for (int i = 0; i < mpgCount; i++)
            {
                  c.nMpList[j] = (short)mpgList.ElementAt(i).Item1;
                  j += 1;
                  c.nMpList[j] = mpgList.ElementAt(i).Item2;
                  j += 1;
            }
            var result = repo.Send((short)enCfgCmnd.enCcMpg, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ConfigureMonitorPointGroup, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.ConfigureMonitorPointGroup,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ConfigureMonitorPointGroup, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.ConfigureMonitorPointGroup,
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

      public CommandResponse MonitorGroupStatusRequest(string mac, short scpId, short mpgId, short count)
      {
            CC_MPGSRQ c = new CC_MPGSRQ
            {
              scp_number = scpId,
              first = mpgId,
              count = count    
            };
            var result = repo.Send((short)enCfgCmnd.enCcMpgSrq, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.MonitorGroupStatusRequest, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.MonitorGroupStatusRequest,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.MonitorGroupStatusRequest, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.MonitorGroupStatusRequest,
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

      public CommandResponse MonitorPointGroupArmDisarmCommand(string mac, short scpId, short mpgId, short command, short arg1)
      {
            CC_MPGSET c = new CC_MPGSET
            {
              scp_number = scpId,
              mpg_number = mpgId,
              command = command,
              arg1 = arg1    
            };
            var result = repo.Send((short)enCfgCmnd.enCcMpgSet, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.MonitorPointGroupArmDisarmCommand, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.MonitorPointGroupArmDisarmCommand,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.MonitorPointGroupArmDisarmCommand, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.MonitorPointGroupArmDisarmCommand,
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