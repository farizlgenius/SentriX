namespace Setting.Infrastructure.Persistences.Entities;

public sealed class WeakPassword : BaseEntity
{
  public string pattern { get; set; } = string.Empty;
  public Guid password_rule_guid { get; set; }
  public PasswordRule password_rule { get; set; } = null!;

  public WeakPassword() { }
  public WeakPassword(Setting.Domain.Entities.WeakPassword d) : base(d.Guid)
  {
    pattern = d.Pattern;
    password_rule_guid = d.PasswordRuleGuid;
  }
}