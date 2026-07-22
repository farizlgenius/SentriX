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
            Guid DeviceGuid,
            Guid DoorGuid,
            string Metadata
            )
      {
            throw new NotImplementedException();
      }

      public async Task DeleteDoorAsync(
            Guid DeviceGuid,
            Guid DoorGuid,
            string Metadata
      )
      {
            throw new NotImplementedException();
      }

      public Task UpdateDoorAsync(
            Guid DeviceGuid,
            Guid DoorGuid,
            string Metadata
      )
      {
            throw new NotImplementedException();
      }
}