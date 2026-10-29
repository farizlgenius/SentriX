using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IAreaRepository
{
      CommandResponse ConfigureAccessArea(
            string mac,
            short scpId,
            short areaNo,
            short multiOccMode,
            short acs,
            short occControl,
            short occSet,
            short occMax,
            short occUp,
            short occDown,
            short areaFlag
      );

      CommandResponse ModifyAccessAreaConfiguration(
            string mac,
            short scpId,
            short areaNo,
            short command,
            short occSet
      );

      CommandResponse AccessAreaStatusRequest(
            string mac,
            short scpId,
            short first,
            short count
      );

}