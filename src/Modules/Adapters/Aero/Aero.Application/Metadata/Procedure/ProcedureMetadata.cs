using Aero.Application.Enums;
using SharedKernel.Enums;

namespace Aero.Application.Metadata.Device;

public sealed class ProcedureMetadata
{
      public Dictionary<ActionType,object> Values {get; set;} = new Dictionary<ActionType, object>();

}