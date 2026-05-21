using Adapter.Aero.Persistences.Entities;
using SharedKernel.Model;

namespace Adapter.Aero.Interfaces;

public interface IOutputCommand
{
      CommandResponse OutputPointSpecification(
            string Mac,
            short ScpId,
            short SioNumber,
            short Output,
            short Mode
      );
      CommandResponse ControlPointConfiguration(
            string Mac,
            short ScpId,
            short CpNumber,
            short SioNumber,
            short OpNumber,
            short DefaultPulse
      );
      CommandResponse ControlPointCommand(short ScpId,string Mac,int CpId,short Command);
      CommandResponse DeleteControlPoint(
             string Mac,
            short ScpId,
            short CpNumber,
            short OpNumber,
            short DefaultPulse
      );


}