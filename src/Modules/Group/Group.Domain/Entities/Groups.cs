using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Group.Domain.Entities;

public sealed class Groups : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public string Metadata {get; private set;} = string.Empty;

      public Groups(int id, short componentId,string name,string metadata,int locationId, bool IsActive) : base(id, componentId, locationId, IsActive)
      {
            ValidationHelper.IsValidName(name);
            this.Name = name;
            this.Metadata = metadata;


      }
}