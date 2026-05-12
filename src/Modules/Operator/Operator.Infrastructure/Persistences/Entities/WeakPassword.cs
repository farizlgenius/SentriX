using System;
using System.ComponentModel.DataAnnotations;

namespace Operator.Infrastructure.Persistences.Entities;

public class WeakPassword
{
      [Key]
      public int id {get; set;}
      public string pattern {get; set;} = string.Empty;
      public int password_rule_id {get; set;}
      public PasswordRule password_rule {get; set;} = null!;

      public WeakPassword(){}

}