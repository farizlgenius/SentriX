using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Face : BaseEntity
{

      public string image_name { get; set; } = string.Empty;
       public Guid? user_guid { get; set; }
      public User user { get; set; } = default!;

      public Face(){}

      public Face(Domain.Entities.Face d) : base(d.Guid)
      {
            this.image_name = d.ImageName;
            this.user_guid = d.UserGuid;
      }

}