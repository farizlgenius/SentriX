using SharedKernel.Domain;

namespace User.Infratructure.Persistences.Entities;

public sealed class UserGroup : BaseEntity
{
      public int user_id { get; set; }
      public Users user { get; set; } = new Users();
      public int group_id { get; set; }
      public UserGroup(){}

}