using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class CardFormatRepository(IBaseRepository repo,ILogger<CardFormatRepository> logger) : ICardFormatRepository
{
      public CommandResponse CardFormatterConfiguration(string mac, short scpId, short cfmtNo, short fac, short offset, short functionId, short flags, short bits, short peLn, short peLoc, short poLn, short poLoc, short fcLn, short fcLoc, short chLn, short chLoc, short icLn, short icLoc)
      {
            CC_SCP_CFMT c = new CC_SCP_CFMT();
            c.lastModified = 0;
            c.nScpID = scpId;
            c.number = cfmtNo;
            c.facility = fac;
            c.offset = offset;
            c.function_id = functionId;
            if(functionId == 1)
            {
                  c.arg.sensor.flags = flags;
                  c.arg.sensor.bits = bits;
                  c.arg.sensor.pe_ln = peLn;
                  c.arg.sensor.pe_loc = peLoc;
                  c.arg.sensor.po_ln = poLn;
                  c.arg.sensor.po_loc = poLoc;
                  c.arg.sensor.fc_ln = fcLn;
                  c.arg.sensor.fc_loc = fcLoc;
                  c.arg.sensor.ch_ln = chLn;
                  c.arg.sensor.ch_loc = chLoc;
                  c.arg.sensor.ic_ln = icLn;
                  c.arg.sensor.ic_loc = icLoc;
            }
            var result = repo.Send((short)enCfgCmnd.enCcScpCfmt, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.CardFormatterConfiguration, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.CardFormatterConfiguration,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.CardFormatterConfiguration, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.CardFormatterConfiguration,
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