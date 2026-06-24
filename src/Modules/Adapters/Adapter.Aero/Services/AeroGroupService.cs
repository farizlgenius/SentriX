using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Events.Contract.Command;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class AeroGroupService(IGroupCommand group,IMessageBus bus) : IGroupAdapter
{
      public async Task CreateUpdateLevel(string Mac, short DeviceComponentId, short ComponentId, List<(short DoorComponentId,short TimeZoneComponentId)> Doors)
      {


            var res = group.AccessLevelConfigurationExtended(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  Doors
            );

            await bus.SendAsync(new AddCommandEvent(res));
            
      }

      public async Task DeleteLevel(string Mac, short DeviceComponentId, short ComponentId)
      {
            var res = group.AccessLevelConfigurationExtended(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  new List<(short DoorComponentId, short TimeZoneComponentId)>()
            );

            await bus.SendAsync(new AddCommandEvent(res));
      }
}