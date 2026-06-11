using System;
using SharedKernel.Domain;

namespace User.Infratructure.Persistences.Entities;

public sealed class Position : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public ICollection<Users> users { get; set; } = new List<Users>();

      public Position() { }


      public Position(string name, string description, int location) : base(0, location, true)
      {
            this.name = name;
            this.description = description;
      }
}
