using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class UserAdditional 
{
      [Key]
      public int id {get; set; }
      public string additional { get; set; } = string.Empty;
      public int user_id { get; set; }
      public User user { get; set; } = default!;

      public UserAdditional() { }


      public UserAdditional(string additional)
      {
            this.additional = additional;
      }
}