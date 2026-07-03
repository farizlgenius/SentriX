using SharedKernel.Domain;

namespace Input.Infrastructure.Persistences.Entities;

public sealed class InputGroupDetail : BaseEntity
{

      public string mac {get; set;} = string.Empty;
      public short device_component_id {get; set;}
      public int input_group_id {get; set;}
      public InputGroups input_group {get; set;} = default!;
      public ICollection<InputList> input_list {get; set;} = default!;
      public InputGroupDetail(){}

      public InputGroupDetail(string mac, short device_component_id, List<InputList> input_list)
      {
            this.mac = mac;
            this.device_component_id = device_component_id;
            this.input_list = input_list;
      }
}