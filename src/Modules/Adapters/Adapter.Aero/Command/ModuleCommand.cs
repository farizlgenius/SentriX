
using Adapter.Aero.Command;
using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Model;

namespace AeroAdapter.Infrastructure.Writer;

public sealed class ModuleCommand(ILogger<ModuleCommand> logger) : BaseCommand, IModuleCommand
{
      public CommandResponse SioPanelConfiguration(
            string Mac,
           short ScpId,
           short SioNumber,
           short nInput,
           short nOutput,
           short nReader,
           short Model,
           short Enable,
           short Port,
           short Address,
           short Emax,
           short Flags,
           short nSioNextIn,
           short nSioNextOut,
           short nSioNextRdr 
      )
      {
            CC_SIO c = new CC_SIO();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.sio_number = SioNumber;
            c.nInputs = nInput;
            c.nOutputs = nOutput;
            c.nReaders = nReader;
            c.model = Model;
            c.revision = 0;
            c.ser_num_low = 0;
            c.ser_num_high = -1;
            c.enable = Enable;
            c.port = Port;
            c.channel_out = 0;
            c.channel_in = 0;
            c.address = Address;
            c.e_max = Emax;
            c.flags = Flags;
            c.nSioNextIn = nSioNextIn;
            c.nSioNextOut = nSioNextOut;
            c.nSioNextRdr = nSioNextRdr;
            c.nSioConnectTest = 0;
            c.nSioOemCode = 0;
            c.nSioOemMask = 0;
            var result = Send((short)enCfgCmnd.enCcSio, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.SioPanelConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.SioPanelConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.SioPanelConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.SioPanelConfiguration,
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

      public CommandResponse SioStatusRequest(string Mac,short ScpId, int First, int Count)
      {
            CC_SIOSRQ c = new CC_SIOSRQ();
            c.scp_number = ScpId;
            c.first = (short)First;
            c.count = (short)Count;
            var result = Send((short)enCfgCmnd.enCcSioSrq, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.SioStatusReq, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.SioStatusReq,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.SioStatusReq, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.SioStatusReq,
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
