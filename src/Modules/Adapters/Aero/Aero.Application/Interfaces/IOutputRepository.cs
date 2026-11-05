using SharedKernel.Enums;
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
            short driveMode
      );

      CommandResponse ControlPointConfiguration(
            string mac,
            short scpId,
             short sioNo,
            short cpNo,
            short outputNo,
            short defaultPulse
      );

      CommandResponse ControlPointCommand(
            string mac,
            short scpId,
            short cpNo,
            OutputCommand command,
            short onTime,
            short offTime,
            short repeat
      );

      CommandResponse ControlPointStatus(
            string mac,
            short scpId,
            short cpNo,
            short count
      );
}