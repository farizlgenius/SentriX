using System;
using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class UserAdditional : BaseDbEntity
{

      public string additional { get; set; } = string.Empty;
      public Guid user_guid { get; set; }
      public Users user { get; set; } = new Users();

      public UserAdditional() { }


      public UserAdditional(string additional)
      {
            this.additional = additional;
      }

}
