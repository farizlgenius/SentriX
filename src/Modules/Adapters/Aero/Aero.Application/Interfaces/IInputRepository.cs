using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IInputRepository
{
      CommandResponse InputPointSpecification(
            string mac,
            short scpId,
            short sioNo,
            short inputNo,
            short icvtNum,
            short debounce,
            short holdTime
      );

      CommandResponse MonitorPointConfiguration(
            string mac,
            short scpId,
            short mpNo,
            short mpSio,
            short inputNo,
            short lfCode,
            short mode,
            short delayEntry,
            short delayExit
      );
}