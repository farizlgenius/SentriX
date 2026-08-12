namespace Setting.Domain.Entities;

public sealed class WeakPassword : BaseDomain
{
  public string Pattern { get; private set; } = string.Empty;
  public Guid PasswordRuleGuid { get; private set; }
  public WeakPassword(
    string pattern,
    Guid passwordRuleGuid
  )
  {
    Pattern = pattern;
    PasswordRuleGuid = passwordRuleGuid;
  }
}