namespace Core.Infrastructure.Persistences.Entities;

public sealed class UserGroup : BaseEntity
{
  public int user_id { get; set; }
  public User user { get; set; } = default!;
  public int group_id { get; set; }
  public Group group { get; set; } = default!;

  public UserGroup() { }
  public UserGroup(
    int userId,
    int groupId
  )
  {
    user_id = userId;
    group_id = groupId;
  }

}