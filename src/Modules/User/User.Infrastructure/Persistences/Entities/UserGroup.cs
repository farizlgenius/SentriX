using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class UserGroup 
{
      [Key]
      public int id {get; set;}
      public Guid guid {get; set;}
      public Guid user_guid { get; set; }
      public Users user { get; set; } = default!;
      public Guid group_guid { get; set; }
      public DateTime created_at { get; set; }
      public DateTime updated_at { get; set; }
      public UserGroup(){}
      public UserGroup(Guid guid,Guid group_guid,Guid user_guid)
      {
            this.guid = guid;
            this.group_guid = group_guid;
            this.user_guid = user_guid;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
      }

}