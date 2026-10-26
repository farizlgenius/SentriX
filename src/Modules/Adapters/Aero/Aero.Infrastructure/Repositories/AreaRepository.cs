using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class AreaRepository(
      IBaseRepository repo,
      ILogger<AreaRepository> logger
      ) : IAreaRepository
{
      public CommandResponse AccessAreaStatusRequest(string mac, short scpId, short first, short count)
      {
            CC_AREASRQ c = new CC_AREASRQ
            {
              scp_number = scpId,
              first = first,
              count = count    
            };
            var result = repo.Send((short)enCfgCmnd.enCcAreaSpc, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.AreaStatusRequest, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.AreaStatusRequest,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.AreaStatusRequest, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.AreaStatusRequest,
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

      public CommandResponse ConfigureAccessArea(string mac, short scpId, short areaNo, short multiOccMode, short acs, short occControl, short occSet, short occMax, short occUp, short occDown, short areaFlag)
      {
            CC_AREA_SPC c = new CC_AREA_SPC
            {
                  lastModified = 0,
                  scp_number = scpId,
                  area_number = areaNo,
                  multi_occupancy = multiOccMode,
                  access_control = acs,
                  occ_control = occControl,
                  occ_set = occSet,
                  occ_max = occMax,
                  occ_up = occUp,
                  occ_down = occDown,
                  area_flags = areaFlag

            };
            var result = repo.Send((short)enCfgCmnd.enCcAreaSpc, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ConfigureAccessArea, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.ConfigureAccessArea,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ConfigureAccessArea, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.ConfigureAccessArea,
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

      public CommandResponse ModifyAccessAreaConfiguration(string mac, short scpId, short areaNo, short command, short occSet)
      {
            CC_AREASET c = new CC_AREASET
            {
              scp_number = scpId,
              area_number = areaNo,
              command = command,
              occ_set = occSet    
            };
            var result = repo.Send((short)enCfgCmnd.enCcAreaSet, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ModifyAccessAreaConfiguration, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.ModifyAccessAreaConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ModifyAccessAreaConfiguration, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.ModifyAccessAreaConfiguration,
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