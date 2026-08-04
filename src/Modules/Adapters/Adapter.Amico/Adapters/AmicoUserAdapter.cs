using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Amico.Adapters;

public sealed class AmicoUserAdapter(
      IDeviceCommand command,
      IAmicoRepository repo,
      IMessageBus bus
      ) : IAmicoUserAdapter
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
            
            
      }


      public Task DeleteAsync(
            Guid DeviceGuid, int CardNumber, string LicenseNumber, string Pin, string QrCode, string ImageName
            )
      {
            throw new NotImplementedException();
      }
}