using Aero.Application.Constants;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class OutputRepository(
      IBaseRepository repo,
      ILogger<OutputRepository> logger
      ) : IOutputRepository
{
      public CommandResponse ControlPointConfiguration(string mac, short scpId, short sioNo, short cpNo, short outputNo, short defaultPulse)
      {
            CC_CP c = new CC_CP();
            c.lastModified = 0;
            c.scp_number = scpId;
            c.cp_number = cpNo;
            c.sio_number = sioNo;
            c.op_number = outputNo;
            c.dflt_pulse = defaultPulse;
            var result = repo.Send((short)enCfgCmnd.enCcCP, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.ControlPointConfiguration, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.ControlPointConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.ControlPointConfiguration, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.ControlPointConfiguration,
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

      public CommandResponse OutputPointSpecification(string mac, short scpId, short sioNo, short outputNo, short offlineMode, short defaultMode)
      {
            CC_OP c = new CC_OP();
            c.lastModified = 0;
            c.scp_number = scpId;
            c.sio_number = sioNo;
            c.output = outputNo;
            c.mode = UtilitiesHelper.FinalizeOutputMode(offlineMode, defaultMode);
            var result = repo.Send((short)enCfgCmnd.enCcOutput, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.OutputPointSpecification, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.OutputPointSpecification,
                        SCPDLL.scpGetTagLastPosted(scpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        ObjectHelper.ToAsciiString(c),
                        CommandStatus.PENDING,
                        string.Empty,
                        SharedKernel.Enums.Vendor.aero,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.OutputPointSpecification, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.OutputPointSpecification,
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