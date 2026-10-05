using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface ICardFormatRepository
{
      CommandResponse CardFormatterConfiguration(
            string mac,
            short scpId,
            short cfmtNo,
            short fac,
            short offset,
            short functionId,
            short flags,
            short bits,
            short peLn,
            short peLoc,
            short poLn,
            short poLoc,
            short fcLn,
            short fcLoc,
            short chLn,
            short chLoc,
            short icLn,
            short icLoc
      );
}