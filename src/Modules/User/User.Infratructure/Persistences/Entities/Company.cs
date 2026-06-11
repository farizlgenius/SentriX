using System;
using SharedKernel.Domain;

namespace User.Infratructure.Persistences.Entities;

public sealed class Company : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string description { get; set; } = string.Empty;
      public string address { get; set; } = string.Empty;
      public ICollection<Users> users { get; set; } = new List<Users>();
      public ICollection<Department> departments { get; set; } = new List<Department>();

      public Company() { }


      public Company(string name,string address,string description,int location) : base(0,location,true)
      {
            this.name = name;
            this.address = address;
            this.description = description;

      }


}
