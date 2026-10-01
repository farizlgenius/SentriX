using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IOutputRepository
{
      CommandResponse OutputPointSpecification(
            string mac,
            short scpId,
            short sioNo,
            short outputNo,
            short offlineMode,
            short defaultMode
      );

      CommandResponse ControlPointConfiguration(
            string mac,
            short scpId,
             short sioNo,
            short cpNo,
            short outputNo,
            short defaultPulse
      );
}