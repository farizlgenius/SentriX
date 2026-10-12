using Aero.Application.Constants;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class TriggerRepository(
      IBaseRepository repo,
      ILogger<TriggerRepository> logger
) : ITriggerRepository
{
      public CommandResponse TriggerSpecification(
            string mac, 
            short scpId,
            short trgId,
            ProcedureCommand procCommand,
            short procId,
            short srcType,
            short srcNo,
            short tranTyp,
             List<short> codeMap,
            short tz,
            short[] trgVar
            )
      {
            CC_TRGR c = new CC_TRGR();
            c.lastModified = 0;
            c.scp_number = scpId;
            c.trgr_number = trgId;
            c.command = (short)procCommand;
            c.proc_num = procId;
            c.src_type = srcType;
            c.src_number = srcNo;
            c.tran_type = tranTyp;
            int re = 0;
            foreach(var code in codeMap)
            {
                  re += (int)Math.Pow(2, code);
            }
            if(codeMap.Count > 0)
            {
                 c.code_map = re; 
            }
            c.timezone = tz;
            for(int i = 0; i < 4; i++)
            {
                  c.trig_var[i] = trgVar.ElementAtOrDefault(i);
            }
            var result = repo.Send((short)enCfgCmnd.enCcScpTimezoneExAct, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.TriggerSpecification, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.TriggerSpecification,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.TriggerSpecification, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.TriggerSpecification,
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