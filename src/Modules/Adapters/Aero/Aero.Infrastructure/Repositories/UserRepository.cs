using Aero.Application.Constants;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Helpers;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class UserRepository(IBaseRepository repo,ILogger<UserRepository> logger) : IUserRepository
{
      public CommandResponse AccessDatabaseCardRecords(string mac, short scpId, short flags, int cardNumber, short issueCode, string pin, List<short> alvl, short apbLoc, int useCount, DateTime actTime, DateTime? dactTime)
      {
            CC_ADBC_I64DTIC32 c = new CC_ADBC_I64DTIC32();
            c.lastModified = 0;
            c.scp_number = scpId;
            c.flags = flags;
            c.card_number = cardNumber;
            c.issue_code = issueCode;
            for(int i = 0;i < pin.Length; i++)
            {
                  c.pin[i] = pin[i];
            }
            for(int i = 0;i < alvl.Count; i++)
            {
                  c.alvl[i] = alvl[i];
            }
            c.apb_loc = apbLoc;
            c.use_count = (short)useCount;
            c.act_time = (int)UtilitiesHelper.DateTimeToElapeSecond(actTime);
            c.dact_time = dactTime == null ? -1 : (int)UtilitiesHelper.DateTimeToElapeSecond(dactTime ?? DateTime.UtcNow.AddYears(10));
            var result = repo.Send((short)enCfgCmnd.enCcAdbCardI64DTic32, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.AccessDatabaseCardRecords, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.AccessDatabaseCardRecords,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.AccessDatabaseCardRecords, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.AccessDatabaseCardRecords,
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

      public CommandResponse CardDelete(
            string mac, short scpId, int cardNumber
      )
      {
            CC_CARDDELETEI64 c = new CC_CARDDELETEI64();
            c.scp_number = scpId;
            c.cardholder_id = cardNumber;
            var result = repo.Send((short)enCfgCmnd.enCcCardDeleteI64, c);
            if (result)
            {
                  logger.LogInformation(LogMessageHelper.CommandSuccess(Command.CardDelete, scpId));

                  return new CommandResponse(
                        mac,
                        scpId,
                        Command.CardDelete,
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
                  logger.LogError(LogMessageHelper.CommandUnsuccess(Command.CardDelete, scpId));
                  return new CommandResponse(
                        mac,
                       scpId,
                       Command.CardDelete,
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