using System;
using SharedKernel.Domain;

namespace User.Infratructure.Persistences.Entities;

public sealed class Department : BaseEntity
{
       public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public ICollection<Users> users { get; set; } = new List<Users>();
      public ICollection<Position> positions { get; set; } = new List<Position>();

      public Department() { }


      public Department(string name, string description,int location) : base(0,location,true)
      {
            this.name = name;
            this.description = description;
      }
}
