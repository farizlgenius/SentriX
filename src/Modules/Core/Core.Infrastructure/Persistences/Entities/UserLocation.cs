namespace Core.Infrastructure.Persistences.Entities;

public sealed class UserLocation : BaseEntity
{
  public int user_id { get; set; }
  public User user { get; set; } = default!;
  public int location_id { get; set; }
  public Location location { get; set; } = default!;

  public UserLocation() { }

  public UserLocation(
    int user
  ) : base(Guid.NewGuid())
  {
    user_id = user;
  }
  public UserLocation(
    int user,
    int loc
  ) : base(Guid.NewGuid())
  {
    user_id = user;
    location_id = loc;
  }
}