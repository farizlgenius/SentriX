using Aero.Application.Enums;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IProcedureRepository
{
      CommandResponse ActionSpecification(
            string mac, 
            short scpId, 
            short procId,
            ActionType type,
            short? externalId,
            string? metadata
            );
      // CommandResponse ActionSpecification(string mac, short scpId, short procId,(ActionType type,string metadata) values);
}