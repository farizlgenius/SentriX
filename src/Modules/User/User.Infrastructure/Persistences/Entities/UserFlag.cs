using System;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class UserFlag : BaseEntity
{
      public string label { get; set; } = string.Empty;
      public int value { get; set; }
      public string description { get; set; } = string.Empty;

      public UserFlag() { }

}
