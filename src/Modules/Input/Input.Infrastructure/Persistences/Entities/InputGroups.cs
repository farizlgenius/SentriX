using SharedKernel.Domain;

namespace Input.Infrastructure.Persistences.Entities;

public sealed class InputGroups : BaseEntity
{
      public string name {get; set;} = string.Empty;
      public string type {get; set;}  = string.Empty;
      public ICollection<InputGroupDetail> input_group_detail {get; set;} = default!;
      
      public InputGroups()
      {
      }

      public InputGroups(Domain.Entities.InputGroups domain)
      {
            this.name = domain.Name;
            this.input_group_detail = domain.InputGroupDetails.Select(x => new InputGroupDetail(
                  x.Mac,
                  x.DeviceComponentId,
                  x.InputList.Select(i => new InputList(
                        i.ComponentId,
                        i.Type
                  )).ToList()
            )).ToList();
            this.type = domain.Type;
      }
}