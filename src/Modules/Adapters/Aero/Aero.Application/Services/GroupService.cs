using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Device;
using Core.Contract.Commands.Events;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class GroupService(
      IGroupRepository repo,
      IMessageBus bus
      ) : IGroupAdapter
{
      public async Task AddAccessGroupAsync(string mac, string ip,short deviceId,short groupId, List<(short doorId, short timeZoneId)> datas)
      {
                        
            var res = repo.AddAccessGroup(
                  mac,
                  deviceId,
                  groupId,
                  0,
                  datas
                  );

            await bus.SendAsync(new AdapterEventCommand(res));
      }
}