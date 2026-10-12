using Aero.Application.Enums;
using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IProcedureRepository
{
      CommandResponse ActionSpecification(string mac, short scpId, short procId,Dictionary<ActionType,object> values);
}