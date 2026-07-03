using SharedKernel.Domain;

namespace Input.Domain.Entities;

public sealed class InputGroups : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public string Type {get; private set;}  = string.Empty;
      public List<InputGroupDetail> InputGroupDetails {get; private set;} = default!;
      
      public InputGroups(
            int id,
            short componentId,
       string name,
       string type,
       List<InputGroupDetail> inputGroupDetails,
       int locationId, bool IsActive) : base(id, componentId, locationId, IsActive)
      {
            Name = name;
            Type = type;
            InputGroupDetails = inputGroupDetails;

      }
}