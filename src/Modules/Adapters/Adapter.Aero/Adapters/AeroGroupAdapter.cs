using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Events.Contract.Command;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Adapters;

public sealed class AeroGroupAdapter(IGroupCommand group, IMessageBus bus) : IAeroGroupAdapter
{
      public async Task CreateGroup(
            string Name,
            short ComponentId,
            List<(string Mac, short DeviceComponentId, short DoorComponentId, short TimeZoneComponentId)> Doors
      )
      {

            var grouped = Doors
                  .GroupBy(d => new
                  {
                        d.Mac,
                        d.DeviceComponentId
                  })
                  .Select(g => new
                  {
                        g.Key.Mac,
                        g.Key.DeviceComponentId,
                        Doors = g.Select(x => new
                        {
                              x.DoorComponentId,
                              x.TimeZoneComponentId
                        }).ToList()
                  })
                  .ToList();

            foreach (var g in grouped)
            {
                  var res = group.AccessLevelConfigurationExtended(
                  g.Mac,
                  g.DeviceComponentId,
                  ComponentId,
                  g.Doors.Select(x => (x.DoorComponentId, x.TimeZoneComponentId)).ToList()
            );

                  await bus.SendAsync(new AddCommandEvent(res));
            }



      }

      public async Task DeleteGroup(string Mac, short DeviceComponentId, short ComponentId)
      {
            var res = group.AccessLevelConfigurationExtended(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  new List<(short DoorComponentId, short TimeZoneComponentId)>()
            );

            await bus.SendAsync(new AddCommandEvent(res));
      }

      public async Task UpdateGroup(string Name, short ComponentId, List<(string Mac, short DeviceComponentId, short DoorComponentId, short TimeZoneComponentId)> Doors)
      {
            var grouped = Doors
                  .GroupBy(d => new
                  {
                        d.Mac,
                        d.DeviceComponentId
                  })
                  .Select(g => new
                  {
                        g.Key.Mac,
                        g.Key.DeviceComponentId,
                        Doors = g.Select(x => new
                        {
                              x.DoorComponentId,
                              x.TimeZoneComponentId
                        }).ToList()
                  })
                  .ToList();

            foreach (var g in grouped)
            {
                  var res = group.AccessLevelConfigurationExtended(
                  g.Mac,
                  g.DeviceComponentId,
                  ComponentId,
                  g.Doors.Select(x => (x.DoorComponentId, x.TimeZoneComponentId)).ToList()
            );

                  await bus.SendAsync(new AddCommandEvent(res));
            }
      }
}