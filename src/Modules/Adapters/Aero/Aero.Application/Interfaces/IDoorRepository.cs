using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IDoorRepository
{
      CommandResponse ReaderSpecification(
            string mac,
            short scpId,
            short sioNo,
            short readerNo,
            short osdpFlag
      );
      CommandResponse AccessControlReaderConfiguration(
            string mac,
            short scpId,
            short acrNo,
            short readerConfig,
            short pairAcr,
            short readerSio,
            short readerNo,
            short relaySio,
            short relayNo,
            short relayMin,
            short relayMax,
            short relayMode,
            short sendorSio,
            short sendorNo,
            short heldOpenDelay,
            short rexSio,
            short rexNo,
            short rex1Sio,
            short rex1No,
            short rex0Mask,
            short rex1Mask,
            short altrRdrSio,
            short altrRdrNo,
            short altrRdrSpec,
            short cdFormat,
            short apbMode,
            short apbIn,
            short apbOut,
            short spare,
            short actlFlags,
            short offlineMode,
            short defaultMode,
            short defaultLedMode,
            short preAalrm,
            short apbDelay
      );

      CommandResponse EnCcAcrSrq(
            string mac,
            short scpId,
            short acrNo
      );

      CommandResponse AcrMode(
            string mac,
            short scpId,
            short acrNo,
            DoorMode mode
      );

      CommandResponse Unlock(
            string mac,
            short scpId,
            short acrNo
      );

      CommandResponse MomentaryUnlock(
            string mac,
            short scpId,
            short acrNo
      );
}