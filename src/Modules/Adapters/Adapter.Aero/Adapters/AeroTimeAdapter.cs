using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Constants;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Adapter.Aero.Persistences.Entities;
using Device.Contract.Queries;
using Events.Contract.Command;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Aero.Adapters;

public sealed class AeroTimeAdapter(
      ITimeCommand time,
      IMessageBus bus,
      IAeroRepository repo
      ) : IAeroTimeAdapter
{
      public Task ClearTimeAsync(
            Guid TzGuid,
          List<Guid> DeviceGuids
      )
      {
            throw new NotImplementedException();
      }

      public async Task CreateHolidayAsync(
            Guid HolidayGuid,
         List<Guid> DeviceGuids,
          string Name,
           DateTime Start,
           DateTime End
            )
      {
            foreach (var g in DeviceGuids)
            {
                  var slots = await repo.GetScpSlotByGuidAsync(g);

                  var dates = DateTimeHelper.ExtractDateFromStartEndDateTime(Start, End);

                  foreach (var date in dates)
                  {
                        var res = time.HolidayConfiguration(
                        slots.mac,
                        (short)slots.slot_id,
                        date.Year,
                        date.Month,
                        date.Day,
                        0,
                        1
                        );

                        await bus.SendAsync(new AddCommandEvent(res));

                  }
            }

            // No slot because holiday is not used slot in SCP

      }

      public async Task CreateTimeZoneAsync(
           Guid TzGuid,
           string Name,
            List<IntervalObject> Intervals,
            List<Guid> DeviceGuids
      )
      {
            var slot = await repo.GetFreeSlotAsync<TzSlot>();
            foreach (var g in DeviceGuids)
            {
                  var deviceSlots = await repo.GetScpSlotByGuidAsync(g);

                  var res = time.ExtendedTimezoneActSpecification(
                        deviceSlots.mac,
                        (short)deviceSlots.slot_id,
                        (short)slot,
                        Intervals
                 );

                  await bus.SendAsync(new AddCommandEvent(res));
            }

            await repo.InsertSlotAsync<TzSlot>(TzGuid, slot);

      }



      public async Task DeleteHolidayAsync(
            Guid HolidayGuid,
          List<Guid> DeviceGuids,
       DateTime Start,
       DateTime End
      )
      {
            foreach (var g in DeviceGuids)
            {
                  var deviceSlot = await repo.GetScpSlotByGuidAsync(g);
                  var dates = DateTimeHelper.ExtractDateFromStartEndDateTime(Start, End);

                  foreach (var date in dates)
                  {
                        var res = time.HolidayConfiguration(
                        deviceSlot.mac,
                        (short)deviceSlot.slot_id,
                        date.Year,
                        date.Month,
                        date.Day,
                        0,
                        0
                        );

                        await bus.SendAsync(new AddCommandEvent(res));
                  }
            }




      }



      public async Task DeleteTimeZoneAsync(
            Guid TzGuid,
          List<short> IntervalComponentId,
          List<Guid> DeviceGuids
      )
      {
            var slot = await repo.GetSlotIdByGuidAsync<TzSlot>(TzGuid);
            foreach (var g in DeviceGuids)
            {
                  var deviceSlot = await repo.GetScpSlotByGuidAsync(g);

                  var res = time.ExtendedTimezoneActSpecification(
                        deviceSlot.mac,
                        (short)deviceSlot.slot_id,
                        (short)slot,
                        new List<IntervalObject>()
                        );

                  await bus.SendAsync(new AddCommandEvent(res));
            }


            await repo.EjectSlotAsync<TzSlot>(slot);
      }


      public async Task UpdateHolidayAsync(
            Guid HolidayGuid, List<Guid> DeviceGuids, DateTime Start, DateTime End
            )
      {
            foreach (var g in DeviceGuids)
            {
                  var deviceSlot = await repo.GetScpSlotByGuidAsync(g);
                  var dates = DateTimeHelper.ExtractDateFromStartEndDateTime(Start, End);

                  foreach (var date in dates)
                  {
                        var res = time.HolidayConfiguration(
                        deviceSlot.mac,
                        (short)deviceSlot.slot_id,
                        date.Year,
                        date.Month,
                        date.Day,
                        0,
                        1
                        );

                        await bus.SendAsync(new AddCommandEvent(res));
                  }
            }

      }

      public async Task UpdateTimeZoneAsync(
            Guid TzGuid,
          string Name, List<IntervalObject> Intervals,
          List<Guid> DeviceGuids
      )
      {
            var slot = await repo.GetSlotIdByGuidAsync<TzSlot>(TzGuid);
            foreach (var g in DeviceGuids)
            {
                  var deviceSlot = await repo.GetScpSlotByGuidAsync(g);

                  var res = time.ExtendedTimezoneActSpecification(
                        deviceSlot.mac,
                        (short)deviceSlot.slot_id,
                        (short)slot,
                        Intervals
                 );

                  await bus.SendAsync(new AddCommandEvent(res));
            }


      }
}