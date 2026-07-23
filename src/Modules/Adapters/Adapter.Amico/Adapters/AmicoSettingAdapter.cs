using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Amico.Adapters;

public sealed class AmicoSettingAdapter(
      IDeviceCommand command,
      IAmicoRepository repo,
      IMessageBus bus
      ) : IAmicoSettingAdapter
{


      public Task CardFormatConfiguration(Guid Guid, string Metadata)
      {
            throw new NotImplementedException();
      }
}