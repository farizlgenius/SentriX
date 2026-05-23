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
      public async Task CreateUpdateLevel(string Mac, short DeviceComponentId, short ComponentId, string Metadata)
      {
            var metadata = JsonSerializer.Deserialize<GroupMetadata>(Metadata);
            if(metadata == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("GroupMetadata"));

            var res = group.AccessLevelConfigurationExtended(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  metadata.Timezones
            );

            await bus.SendAsync(new AddCommandEvent(res));
            
      }

      public async Task DeleteLevel(string Mac, short DeviceComponentId, short ComponentId)
      {
            var res = group.AccessLevelConfigurationExtended(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  new short[64]
            );

            await bus.SendAsync(new AddCommandEvent(res));
      }
}