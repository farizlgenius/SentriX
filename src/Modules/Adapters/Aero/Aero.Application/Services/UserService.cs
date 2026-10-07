using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Core.Contract.Commands.Events;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class UserService(
      IMessageBus bus,
      IUserRepository user
) : IUserAdapter
{
      public async Task AddUserAsync(string mac, string ip, short deviceId, int cardNo, short issueCode, string pin, List<short> alvl, short apbLoc, short useCount, DateTime actTime, DateTime dactTime, CancellationToken ct = default)
      {
            // Metadata
            
            var res = user.AccessDatabaseCardRecords(
                  mac,
                  deviceId,
                  1,
                  cardNo,
                  issueCode,
                  pin,
                  alvl,
                  apbLoc,
                  useCount,
                  actTime,
                  dactTime
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }
}