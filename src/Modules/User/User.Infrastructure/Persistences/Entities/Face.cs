using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class Face 
{

      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public string image_name { get; set; } = string.Empty;
      public DateTime created_at {get; set;}
      public DateTime updated_at {get; set;}
       public Guid? user_guid { get; set; }
      public Users user { get; set; } = default!;

      public Face(){}

      public Face(Domain.Entities.Face d)
      {
            this.guid = d.Guid;
            this.image_name = d.ImageName;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
            this.user_guid = d.UserGuid;
      }


      public void Update(Domain.Entities.Face d)
      {

            this.image_name = d.ImageName;
            this.updated_at = DateTime.UtcNow;
      }
}