using System.IO.Compression;
using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Adapter.Aero.Persistences.Entities;
using Events.Contract.Command;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Adapters;

public sealed class AeroGroupAdapter(
      IAeroRepository repo,
      IGroupCommand group, 
      IMessageBus bus
      ) : IAeroGroupAdapter
{
      public async Task CreateGroup(
            Guid Guid,
            string Name,
            List<(Guid DeviceGuid,Guid DoorGuid,Guid TzGuid)> Doors
      )
      {

            var slot = await repo.GetFreeSlotAsync<GroupSlot>();


            var grouped = Doors
                  .GroupBy(d => new
                  {
                        d.DeviceGuid
                  })
                  .Select(g => new
                  {
                        g.Key.DeviceGuid,
                        Doors = g.Select(x => new
                        {
                              x.DoorGuid,
                              x.TzGuid
                        }).ToList()
                  })
                  .ToList();

            foreach (var g in grouped)
            {
                  var deviceSlot = await repo.GetScpSlotByGuidAsync(g.DeviceGuid);
                  var doorTask = g.Doors.Select(async x =>
                  {
                        var doorSlots = await repo.GetSlotIdsByGuidAsync<AcrSlot>(x.DoorGuid);
                        var tzSlot = await repo.GetSlotIdByGuidAsync<TzSlot>(x.TzGuid);

                        return doorSlots.Select(x => new
                        {
                              DoorComponentId=x,
                              TimeZoneComponentId=tzSlot
                        });   
                  });

                  var doorsResults = await Task.WhenAll(doorTask);

                  var list = doorsResults.SelectMany(items => items.Select(x => ((short)x.DoorComponentId,(short)x.TimeZoneComponentId)).ToList()).ToList();

                  var res = group.AccessLevelConfigurationExtended(
                        deviceSlot.mac,
                        (short)deviceSlot.slot_id,
                        (short)slot,
                        list
                  );

                  await bus.SendAsync(new AddCommandEvent(res));
            }

            await repo.InsertSlotAsync<GroupSlot>(
                  Guid,
                  slot
            );


      }

      public async Task DeleteGroup(
            Guid GroupGuid,
            List<Guid> DeviceGuids
            )
      {
            var slotId = await repo.GetSlotIdByGuidAsync<GroupSlot>(GroupGuid);

            foreach (var deviceGuid in DeviceGuids)
            {
                  var deviceSlot = await repo.GetScpSlotByGuidAsync(deviceGuid);

                  var res = group.AccessLevelConfigurationExtended(
                        deviceSlot.mac,
                        (short)deviceSlot.slot_id,
                        (short)slotId,
                        new List<(short DoorComponentId, short TimeZoneComponentId)>()
                  );

                  await bus.SendAsync(new AddCommandEvent(res));
            }


            await repo.EjectSlotAsync<GroupSlot>(
                  GroupGuid,
                  slotId
            );
      }

      public async Task UpdateGroup(
            Guid Guid,
            string Name,
            List<(Guid DeviceGuid,Guid DoorGuid,Guid TzGuid)> Doors
      )
      {
            var slot = await repo.GetSlotIdByGuidAsync<GroupSlot>(Guid);
            var grouped = Doors
                  .GroupBy(d => new
                  {
                        d.DeviceGuid
                  })
                  .Select(g => new
                  {
                        g.Key.DeviceGuid,
                        Doors = g.Select(x => new
                        {
                              x.DoorGuid,
                              x.TzGuid
                        }).ToList()
                  })
                  .ToList();

            foreach (var g in grouped)
            {
                  var deviceSlot = await repo.GetScpSlotByGuidAsync(g.DeviceGuid);
                  var doorTask = g.Doors.Select(async x =>
                  {
                        var doorSlots = await repo.GetSlotIdsByGuidAsync<AcrSlot>(x.DoorGuid);
                        var tzSlot = await repo.GetSlotIdByGuidAsync<TzSlot>(x.TzGuid);

                        return doorSlots.Select(x => new
                        {
                              DoorComponentId=x,
                              TimeZoneComponentId=tzSlot
                        });   
                  });

                  var doorsResults = await Task.WhenAll(doorTask);

                  var list = doorsResults.SelectMany(items => items.Select(x => ((short)x.DoorComponentId,(short)x.TimeZoneComponentId)).ToList()).ToList();

                  var res = group.AccessLevelConfigurationExtended(
                        deviceSlot.mac,
                        (short)deviceSlot.slot_id,
                        (short)slot,
                        list
                  );

                  await bus.SendAsync(new AddCommandEvent(res));
            }


      }
}