using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using Events.Contract.Command;
using SharedKernel.Messaging;

namespace Adapter.Aero.Adapters;

public sealed class AeroUserAdapter(IUserCommand user, IMessageBus bus) : IAeroUserAdapter
{
      public async Task AddUserAsync(string Mac, short DeviceComponentId, string Identification, string Name, int Active, int Expire, int Card, string License, string Pin, string QrCode, string FaceFile, List<short> Groups)
      {
            var res = user.AccessDatabaseCardRecords(
                  Mac,
                  DeviceComponentId,
                  Card,
                  Pin,
                  Groups,
                  Active,
                  Expire
                  );

            await bus.SendAsync(new AddCommandEvent(res));
      }


      public async Task DeleteUserAsync(string Mac, short ScpId, int CardNumber, string LicenseNumber, string Pin, string QrCode, string ImageName)
      {
            var res = user.CardDelete(
                  Mac,
                  ScpId,
                  CardNumber
                  );

            await bus.SendAsync(new AddCommandEvent(res));
      }
}