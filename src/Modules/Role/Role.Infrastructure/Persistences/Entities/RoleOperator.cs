using System;
using SharedKernel.Domain;

namespace Role.Infrastructure.Persistences.Entities;

public class RoleOperator : BaseEntity
{
      public int role_id {get; set;}
      public Roles role {get; set;} = new Roles();
      public int operator_id {get; set;}

}
