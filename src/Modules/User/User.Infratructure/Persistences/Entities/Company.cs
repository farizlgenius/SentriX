using System;
using SharedKernel.Domain;

namespace User.Infratructure.Persistences.Entities;

public sealed class Company : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public ICollection<Users> users { get; set; } = new List<Users>();
      public int location_id {get; set;}

      public Company() { }


      public Company(string name, string description,int location)
      {
            this.name = name;
            this.description = description;
            this.location_id = location;

      }


}
