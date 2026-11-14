using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface ITriggerRepository
{
      CommandResponse TriggerSpecification(
           string mac, 
            short scpId,
            short trgId,
            TriggerCommand procCommand,
            short procId,
            short srcType,
            short srcNo,
            short tranTyp,
             IEnumerable<short> codeMap,
            short tz,
            short[] trgVar,
            short[] args
            );
}