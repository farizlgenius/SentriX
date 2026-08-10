namespace Core.Infrastructure.Persistences.Entities;

public sealed class UserAdditional : BaseEntity
{
       public string additional { get; set; } = string.Empty;
      public Guid user_guid { get; set; }
      public User user { get; set; } = default!;

      public UserAdditional() { }


      public UserAdditional(string additional)
      {
            this.additional = additional;
      }
}