using Adapter.Contract.Interfaces;
using Aero.Application.Enums;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.User;
using Core.Contract.Commands.Events;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class UserService(
      IMessageBus bus,
      IUserRepository user
) : IUserAdapter
{

      public async Task AddUserAsync(string mac, string ip, short deviceId, int cardNo, string pin, List<short> alvl, DateTime actTime, DateTime dactTime, string metadata, CancellationToken ct = default)
      {
            // Metadata
            var meta = JsonHelper.Deserialize<UserMetadata>(metadata);
            if (meta == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("UserMetadata"));

            var flag = UserFlag.Active;
            if(meta.OneFreeApb) flag |= UserFlag.OneFreeApb;
            if(meta.ApbExempt) flag |= UserFlag.ApbExempt;
            if(meta.PinExempt) flag |= UserFlag.PinExempt;
            
            var res = user.AccessDatabaseCardRecords(
                  mac,
                  deviceId,
                  (short)flag,
                  cardNo,
                  meta.IssueCode,
                  pin,
                  alvl,
                  meta.ApbLoc,
                  meta.UseCount,
                  actTime,
                  dactTime
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }

      public async Task DeleteUserAsync(string mac,string ip,short deviceId,int cardNumber,CancellationToken ct = default)
      {
            var res = user.CardDelete(
                  mac,
                  deviceId,
                  cardNumber
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }
}