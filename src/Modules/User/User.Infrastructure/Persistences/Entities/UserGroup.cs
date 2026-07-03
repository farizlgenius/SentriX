using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class UserGroup : BaseEntity
{
      public int user_id { get; set; }
      public Users user { get; set; } = default!;
      public int group_id { get; set; }
      public UserGroup(){}
      public UserGroup(int groupid,int locationid,bool isactive) : base(0,locationid,isactive,false)
      {
            this.group_id = groupid;
      }

}