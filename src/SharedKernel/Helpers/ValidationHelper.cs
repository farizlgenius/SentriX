using System;
using System.Text.RegularExpressions;

namespace SharedKernel.Helpers;

public static partial class ValidationHelper
{
  [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",RegexOptions.Compiled | RegexOptions.IgnoreCase)]
  private static partial Regex EmailRegex();

  [GeneratedRegex(@"^[a-zA-Z0-9]*$")]
  public static partial Regex CharAndDigitRegex();

  [GeneratedRegex(@"^[\p{L}\p{M}\p{N} ()]+$")]
  private static partial Regex NameRegex();



  public static void IsValidEmail(string? email,string parameterName)
  {
    if (string.IsNullOrWhiteSpace(email))
      throw new ArgumentException($"'{email}' cannot be null or empty.", parameterName);

    if (!EmailRegex().IsMatch(email))
      throw new ArgumentException($"'{email}' email format incorrect.", parameterName);
  }

  public static void IsValidOnlyCharAndDigit(string name,string parameterName)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException($"'{name}' cannot be null or empty.", parameterName);

    if (!CharAndDigitRegex().IsMatch(name))
      throw new ArgumentException($"'{name}' only char and digit support.", parameterName);
  }

  public static void IsValidName(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException($"'{name}' cannot be null or empty.", name);

    if (!NameRegex().IsMatch(name))
      throw new ArgumentException($"'{name}' format incorrect.", name);
  }

  public static void IsNullOrEmpty(string value,string paramter)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException($"'{value}' cannot be null or empty.", paramter);
    }
  }

  public static void ValidateNotMinus(int value, string parameterName)
  {
    if (value < 0)
    {
      throw new ArgumentException($"'{parameterName}' cannot be zero.", parameterName);
    }
  }

  public static bool ValidateTenants(string Tenants, int LocationId)
  {
    if (string.IsNullOrWhiteSpace(Tenants))
      return false;

    var arr = Tenants.Split(",").Select(x => int.Parse(x)).ToList();

    return arr.Contains(LocationId);
  }

}
