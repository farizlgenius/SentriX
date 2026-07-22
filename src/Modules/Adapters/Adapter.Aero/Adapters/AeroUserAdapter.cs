using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences.Entities;
using Events.Contract.Command;
using SharedKernel.Messaging;

namespace Adapter.Aero.Adapters;

public sealed class AeroUserAdapter(
      IUserCommand user,
      IMessageBus bus,
      IAeroRepository repo
      ) : IAeroUserAdapter
{
      public async Task CreateAsync(
            Guid DeviceGuid,
            string Identification,
            string Name,
            int Active,
            int Expire,
            int Card,
            string LicensePlate,
            string Pin,
            string QrCode,
            string FaceFile,
            List<Guid> Groups
            )
      {
            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
            var groupsTask = Groups.Select(async x =>
            {
                  return (short)(await repo.GetSlotIdByGuidAsync<GroupSlot>(x));
            });

            var groups = (await Task.WhenAll(groupsTask)).ToList();

            var res = user.AccessDatabaseCardRecords(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  Card,
                  Pin,
                  groups,
                  Active,
                  Expire
            );

            await bus.SendAsync(new AddCommandEvent(res));

      }


      public async Task DeleteAsync(
            Guid DeviceGuid, 
            int CardNumber, 
            string LicensePlate, 
            string Pin, 
            string QrCode, 
            string ImageName)
      {
            var deviceSlot = await repo.GetScpSlotByGuidAsync(DeviceGuid);
            var res = user.CardDelete(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  CardNumber
                  );

            await bus.SendAsync(new AddCommandEvent(res));
      }
}