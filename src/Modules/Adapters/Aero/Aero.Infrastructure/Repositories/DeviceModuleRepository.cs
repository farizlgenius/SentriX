using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class DeviceModuleRepository(
      IBaseRepository repo,
      ILogger<DeviceModuleRepository> logger
) : IDeviceModuleRepository
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
            var result = repo.Send((short)enCfgCmnd.enCcSio, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.SioPanelConfiguration, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.SioPanelConfiguration,
                        SCPDLL.scpGetTagLastPosted(ScpId),
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.SioPanelConfiguration, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.SioPanelConfiguration,
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

      public CommandResponse SioStatusRequest(string Mac,short ScpId, int First, int Count)
      {
            CC_SIOSRQ c = new CC_SIOSRQ();
            c.scp_number = ScpId;
            c.first = (short)First;
            c.count = (short)Count;
            var result = repo.Send((short)enCfgCmnd.enCcSioSrq, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.SioStatusReq, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        Command.SioStatusReq,
                        SCPDLL.scpGetTagLastPosted(ScpId),
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.SioStatusReq, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       Command.SioStatusReq,
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