using System;
using System.ComponentModel.DataAnnotations;

namespace Operator.Infrastructure.Persistences.Entities;

public sealed class PasswordRule
{
      [Key]
      public int id {get; set;}
      public int len {get;set;}
      public bool is_digit {get; set;}
      public bool is_lower {get; set;}
      public bool is_symbol {get; set;}
      public bool is_upper {get; set;}
      public ICollection<WeakPassword> weaks {get; set;} = new List<WeakPassword>();
      public PasswordRule(){}
      public void Update(Domain.Entities.PasswordRule domain)
      {
            len = domain.Len;
            is_digit = domain.IsDigit;
            is_lower = domain.IsLower;
            is_symbol = domain.IsSymbol;
            is_upper = domain.IsUpper;
            List<string> Old = weaks.Select(x => x.pattern).ToList();
            List<string> Pattern = domain.Weaks.Where(x => Old.Contains(x)).ToList();
            weaks = Pattern.Select(x => new WeakPassword
            {
                  pattern = x
            }).ToList();
      }

}