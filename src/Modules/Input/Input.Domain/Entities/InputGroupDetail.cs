using SharedKernel.Domain;

namespace Input.Domain.Entities;

public sealed class InputGroupDetail : BaseDomain
{
      public string Mac {get; private set;} = string.Empty;
      public short DeviceComponentId {get; private set;}
      public List<InputList> InputList {get; private set;} = default!;
      public InputGroupDetail(int id,
       string mac,
       short deviceComponentId,
       List<InputList> inputList,
       int locationId, bool IsActive) : base(id, 0, locationId, IsActive)
      {

            Mac = mac;
            DeviceComponentId = deviceComponentId; 
            InputList = inputList;

      }
}