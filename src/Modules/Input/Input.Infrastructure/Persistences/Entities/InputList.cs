using SharedKernel.Domain;

namespace Input.Infrastructure.Persistences.Entities;

public sealed class InputList : BaseEntity
{

      public short input_component_id { get; set; }
      public short input_type { get; set; }
      public int input_group_detail_id {get; set;}
      public InputGroupDetail input_group_detail {get; set;} = default!;

      public InputList() { }

      public InputList(short input_component_id, short input_type)
      {
            this.input_component_id = input_component_id;
            this.input_type = input_type;
      }

}