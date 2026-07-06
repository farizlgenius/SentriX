using SharedKernel.Model;

namespace Adapter.Aero.Interfaces;

public interface ISettingCommand
{
      CommandResponse CardFormatterConfiguration(
            string Mac,
            short ScpId,
            short ComponentId,
            short Fac,
            short Offset,
            short FunctionId,
            short Flags,
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
      );
}