namespace Setting.Infrastructure.Persistences.Entities;

public sealed class PasswordRule : BaseEntity
{
  public int len { get; set; }
  public bool is_digit { get; set; }
  public bool is_lower { get; set; }
  public bool is_symbol { get; set; }
  public bool is_upper { get; set; }
  public ICollection<WeakPassword> weaks { get; set; } = new List<WeakPassword>();
  public PasswordRule() { }
  public PasswordRule(Setting.Domain.Entities.PasswordRule d) : base(d.Guid)
  {
    len = d.Len;
    is_digit = d.IsDigit;
    is_lower = d.IsLower;
    is_symbol = d.IsSymbol;
    is_upper = d.IsUpper;
  }
}