using System;
using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infratructure.Persistences.Entities;

public sealed class UserAdditional : BaseEntity
{

      public string additional { get; set; } = string.Empty;
      public int user_id { get; set; }
      public Users user { get; set; } = new Users();

      public UserAdditional() { }


      public UserAdditional(int userid, string additional)
      {
            this.user_id = userid;
            this.additional = additional;
      }

}
