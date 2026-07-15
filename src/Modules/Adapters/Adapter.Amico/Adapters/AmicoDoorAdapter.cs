using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Amico.Adapters;

public sealed class AmicoDoorAdapter(
      IDeviceCommand command,
      IAmicoRepository repo,
      IMessageBus bus
      ) : IAmicoDoorAdapter
{
      public Task CreateUpdateDoorAsync(
            string Mac, 
            short DeviceComponentId, 
            string Metadata, 
            short FirstComponentId, 
            short SecondComponentId = -1)
      {
            throw new NotImplementedException();
      }

      public async Task DeleteDoorAsync(string Mac, short DeviceComponentId, string Metadata, short FirstComponentId, short SecondComponentId = -1)
      {
            throw new NotImplementedException();
      }

      public Task UpdateDoorAsync(string Mac, short DeviceComponentId, string Metadata, short FirstComponentId, short SecondComponentId = -1)
      {
            throw new NotImplementedException();
      }
}