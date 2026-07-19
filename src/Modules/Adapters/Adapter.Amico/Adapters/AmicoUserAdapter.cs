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
      public Task AddUserAsync(
             string Mac,
          short DeviceComponentId,
          string Identification,
          string Name,
          int Active,
          int Expire,
          int Card,          
          string License,
          string Pin,
          string QrCode,
          string FaceFile,
          List<short> Groups
      )
      {
            throw new NotImplementedException();
      }


      public Task DeleteUserAsync(string Mac, short ScpId, int CardNumber, string LicenseNumber, string Pin, string QrCode, string ImageName)
      {
            throw new NotImplementedException();
      }
}