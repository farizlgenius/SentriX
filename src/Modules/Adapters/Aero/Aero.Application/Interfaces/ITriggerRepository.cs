using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface ITriggerRepository
{
      CommandResponse TriggerSpecification(
            string mac, 
            short scpId,
            short trgId,
            ProcedureCommand procCommand,
            short procId,
            short srcType,
            short srcNo,
            short tranTyp,
             List<short> codeMap,
            short tz,
            short[] trgVar
            );
}