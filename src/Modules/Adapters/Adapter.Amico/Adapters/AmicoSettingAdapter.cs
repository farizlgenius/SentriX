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
      public Task CardFormatConfiguration(string Mac, short ScpId, short ComponentId, short Fac, short Offset, short FunctionId, short Flag, short Bits, short PeLn, short PeLoc, short PoLn, short PoLoc, short FcLn, short FcLoc, short ChLn, short ChLoc, short IcLn, short IcLoc)
      {
            throw new NotImplementedException();
      }
}