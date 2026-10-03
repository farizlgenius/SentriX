using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class ModuleRepository(
      IBaseRepository repo,
      ILogger<ModuleRepository> logger
) : IModuleRepository
{
      public CommandResponse EnCcSioSrq(
            string mac,
            short scpId,
            short sioNo,
            CancellationToken ct = default)
      {
            CC_SIOSRQ c = new CC_SIOSRQ();
            c.scp_number = scpId;
            c.first = sioNo;
            c.count = 1;
             var result = repo.Send((short)enCfgCmnd.enCcSioSrq, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.SioStatusReq, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.SioStatusReq,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.SioStatusReq, scpId));
                  return new CommandResponse(
                        mac,
                        scpId,
                       Command.SioStatusReq,
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