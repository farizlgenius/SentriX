using SharedKernel.Domain;

namespace Setting.Contract.DTOs;


public sealed record CreateCardFormatDto(
      string Name = "",
      short Fac = -1,
      short Offset = -1,
      short FunctionId = -1,
      short Flag = -1,
      short Bits = -1,
      short PeLn = -1,
      short PeLoc = -1,
      short PoLn = -1,
      short PoLoc = -1,
      short FcLn = -1,
      short FcLoc = -1,
      short ChLn = -1,
      short ChLoc = -1,
      short IcLn = -1,
      short IcLoc = -1,
      int LocationId = 0,
      bool IsActive = true
) : BaseDto(0,LocationId,"",IsActive);