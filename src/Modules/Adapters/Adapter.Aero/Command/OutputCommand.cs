using Adapter.Aero.Command;
using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Model;

namespace Adapter.Aero.Command;

public sealed class OutputCommand(ILogger<OutputCommand> logger) : BaseCommand,IOutputCommand
{
      public CommandResponse OutputPointSpecification(
            string Mac,
            short ScpId,
            short SioNumber,
            short Output,
            short Mode
      )
      {
            CC_OP c = new CC_OP();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.sio_number = SioNumber;
            c.output = Output;
            c.mode = Mode;
            var result = Send((short)enCfgCmnd.enCcOutput,c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.OutputPointSpecification, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.OutputPointSpecification,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.OutputPointSpecification, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.OutputPointSpecification,
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

      public CommandResponse ControlPointConfiguration(
            string Mac,
            short ScpId,
            short CpNumber,
            short SioNumber,
            short OpNumber,
            short DefaultPulse
      )
      {
            CC_CP c = new CC_CP();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.cp_number = CpNumber;
            c.sio_number = SioNumber;
            c.op_number = OpNumber;
            c.dflt_pulse = DefaultPulse;
             var result = Send((short)enCfgCmnd.enCcCP,c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ControlPointConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ControlPointConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ControlPointConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ControlPointConfiguration,
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

      public CommandResponse ControlPointCommand(
            short ScpId,string Mac,int CpId,short Command
      )
      {
             CC_CPCTL c = new CC_CPCTL();
            c.scp_number = (short)ScpId;
            c.cp_number = (short)CpId;
            c.command = Command;
             var result = Send((short)enCfgCmnd.enCcCpCtl,c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ControlPointCommand, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ControlPointCommand,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ControlPointCommand, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ControlPointCommand,
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

      public CommandResponse DeleteControlPoint(
             string Mac,
            short ScpId,
            short CpNumber,
            short OpNumber,
            short DefaultPulse
      )
      {
            CC_CP c = new CC_CP();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.cp_number = CpNumber;
            c.sio_number = -1;
            c.op_number = OpNumber;
            c.dflt_pulse = DefaultPulse;
            var result = Send((short)enCfgCmnd.enCcCP,c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ControlPointConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ControlPointConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ControlPointConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ControlPointConfiguration,
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