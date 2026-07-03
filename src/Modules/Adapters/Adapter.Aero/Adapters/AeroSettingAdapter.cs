using Adapter.Aero.Interfaces;

namespace Adapter.Aero.Adapters;

public sealed class AeroSettingAdapter : IAeroSettingAdapter
{
      public Task CardFormatConfiguration(
            string Mac,
            short ScpId,
            short ComponentId,
            short Offset,
            short FunctionId,
            short Flag,
            short Bits,
            short PeLn,
            short PeLoc,
            short PoLn,
            short PoLoc,
            short FcLn,
            short FcLoc,
            short ChLn,
            short ChLoc,
            short IcLn,
            short IcLoc
      )
      {
            throw new NotImplementedException();
      }
}