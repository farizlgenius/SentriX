using SharedKernel.Helpers;

namespace Setting.Domain.Entities;

public sealed class PasswordRule : BaseDomain
{
  public int Len { get; private set; }
  public bool IsDigit { get; private set; }
  public bool IsLower { get; private set; }
  public bool IsSymbol { get; private set; }
  public bool IsUpper { get; private set; }
  public List<string> WeakPassword { get; private set; } = default!;
  public PasswordRule(
    int len,
    bool digit,
    bool lower,
    bool symbol,
    bool upper,
    List<string> weaks
  )
  {
    ValidationHelper.NotMinus(len, nameof(Len));
    Len = len;
    IsDigit = digit;
    IsLower = lower;
    IsSymbol = symbol;
    IsUpper = upper;
    WeakPassword = weaks;
  }

  public PasswordRule(
    Guid guid,
    int len,
    bool digit,
    bool lower,
    bool symbol,
    bool upper,
    List<string> weaks
  ) : base(guid)
  {
    ValidationHelper.NotMinus(len, nameof(Len));
    Len = len;
    IsDigit = digit;
    IsLower = lower;
    IsSymbol = symbol;
    IsUpper = upper;
    WeakPassword = weaks;
  }
}