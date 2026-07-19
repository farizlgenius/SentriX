using Adapter.Aero.Constants;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Helpers;
using SharedKernel.Model;

namespace Adapter.Aero.Command;

public sealed class UserCommand(ILogger<UserCommand> logger) : BaseCommand, IUserCommand
{
      // public CommandResponse AccessDatabaseCardRecords(
      //       string Mac, 
      //       short ScpId, 
      //       int Flags, 
      //       long CardNumber, 
      //       short IssueCode, 
      //       string Pin, 
      //       List<short> Groups, 
      //       short ApbLoc, 
      //       short UseCount, 
      //       int ActiveDate,
      //       int DeactiveDate, 
      //       int VacDate, 
      //       short VacDays, 
      //       int TmpDate, short TmpDays)
      // {
      //       CC_ADBC_I64DTIC32 c = new CC_ADBC_I64DTIC32();
      //       c.lastModified = 0;
      //       c.scp_number = ScpId;
      //       c.flags = (short)Flags;
      //       c.card_number = CardNumber;
      //       c.issue_code = IssueCode;
      //       int i = 0;
      //       foreach(var ch in Pin)
      //       {
      //             c.pin[i] = ch;
      //             i++;
      //       }
      //       int j = 0;
      //       foreach(var g in Groups)
      //       {
      //             c.alvl[j] = g;
      //             j++;
      //       }
      //       c.apb_loc = ApbLoc;
      //       c.use_count = UseCount;
      //       c.act_time = ActiveDate;
      //       c.dact_time = DeactiveDate;
      //       c.vac_date = (short)VacDate;
      //       c.vac_days = VacDays;
      //       c.tmp_date = (short)TmpDate;
      //       c.tmp_days = TmpDays;
      //       var result = Send((short)enCfgCmnd.enCcAdbCardI64DTic32, c);
      //       if (result)
      //       {
      //             logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ExtendedTimeZoneActSpecification, ScpId));

      //             return new CommandResponse(
      //                   Mac,
      //                   ScpId,
      //                   CommandConstant.ExtendedTimeZoneActSpecification,
      //                   SCPDLL.scpGetTagLastPosted(ScpId),
      //                   DateTime.UtcNow,
      //                   DateTime.UtcNow,
      //                   ObjectHelper.ToAsciiString(c),
      //                   CommandStatus.PENDING.ToString(),
      //                   string.Empty,
      //                   true
      //                   );

      //       }
      //       else
      //       {
      //             logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ExtendedTimeZoneActSpecification, ScpId));
      //             return new CommandResponse(
      //                   Mac,
      //                  ScpId,
      //                  CommandConstant.ExtendedTimeZoneActSpecification,
      //                  -1,
      //                  DateTime.UtcNow,
      //                  DateTime.UtcNow,
      //                   ObjectHelper.ToAsciiString(c),
      //                  CommandStatus.FAILED.ToString(),
      //                  string.Empty,
      //                  false
      //                  );


      //       }
      // }

      public CommandResponse AccessDatabaseCardRecords(
            string Mac, 
            short ScpId, 
            long CardNumber, 
            string Pin, 
            List<short> Groups, 
            int ActiveDate,
            int DeactiveDate
            )
      {
            CC_ADBC_I64DTIC32 c = new CC_ADBC_I64DTIC32();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.flags = (short)0x01;
            c.card_number = CardNumber;
            c.issue_code = 0;
            int i = 0;
            foreach(var ch in Pin)
            {
                  c.pin[i] = ch;
                  i++;
            }
            int j = 0;
            foreach(var g in Groups)
            {
                  c.alvl[j] = g;
                  j++;
            }
            c.act_time = ActiveDate;
            c.dact_time = DeactiveDate;
            var result = Send((short)enCfgCmnd.enCcAdbCardI64DTic32, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ExtendedTimeZoneActSpecification, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ExtendedTimeZoneActSpecification,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        ObjectHelper.ToAsciiString(c),
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ExtendedTimeZoneActSpecification, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ExtendedTimeZoneActSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                        ObjectHelper.ToAsciiString(c),
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );


            }
      }


      public CommandResponse CardDelete(string Mac, short ScpId, long CardNumber)
      {
            CC_CARDDELETEI64 c = new CC_CARDDELETEI64();
            c.scp_number = ScpId;
            c.cardholder_id = CardNumber;
             var result = Send((short)enCfgCmnd.enCcCardDeleteI64, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(CommandConstant.ExtendedTimeZoneActSpecification, ScpId));

                  return new CommandResponse(
                        Mac,
                        ScpId,
                        CommandConstant.ExtendedTimeZoneActSpecification,
                        SCPDLL.scpGetTagLastPosted(ScpId),
                        DateTime.UtcNow,
                        DateTime.UtcNow,
                        ObjectHelper.ToAsciiString(c),
                        CommandStatus.PENDING.ToString(),
                        string.Empty,
                        true
                        );

            }
            else
            {
                  logger.LogError(LogMessageHelper.CommandUnsuccess(CommandConstant.ExtendedTimeZoneActSpecification, ScpId));
                  return new CommandResponse(
                        Mac,
                       ScpId,
                       CommandConstant.ExtendedTimeZoneActSpecification,
                       -1,
                       DateTime.UtcNow,
                       DateTime.UtcNow,
                        ObjectHelper.ToAsciiString(c),
                       CommandStatus.FAILED.ToString(),
                       string.Empty,
                       false
                       );


            }

      }
}

