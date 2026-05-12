using System;

namespace Operator.Domain.Entities;

public sealed class PasswordRule
{
      public int Id {get; private set;}
      public int Len {get; private set;}
      public bool IsDigit {get; private set;}
      public bool IsLower {get; private set;}
      public bool IsSymbol {get; private set;}
      public bool IsUpper {get; private set;}
      public List<string> Weaks {get; private set;} = new List<string>();

      public PasswordRule(int id,int len,bool digit,bool lower,bool symbol,bool upper,List<string> weaks)
      {
            this.Id = id;
            this.Len = len;
            this.IsDigit =digit;
            this.IsLower = lower;
            this.IsSymbol = symbol;
            this.IsUpper = upper;
            this.Weaks = weaks;

      }

}
