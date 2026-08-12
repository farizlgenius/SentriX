using System;
using System.Text.RegularExpressions;
using SharedKernel.Enums;

namespace SharedKernel.Helpers;

public static partial class ValidationHelper
{
  [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
  private static partial Regex EmailRegex();

  [GeneratedRegex(@"^[a-zA-Z0-9]*$")]
  private static partial Regex CharAndDigitRegex();

  [GeneratedRegex(@"^[0-9]*$")]
  private static partial Regex DigitRegex();

  [GeneratedRegex(@"^[\p{L}\p{M}\p{N} ()]+$")]
  private static partial Regex NameRegex();



  public static void Email(string? email, string parameterName)
  {
    if (string.IsNullOrWhiteSpace(email))
      throw new ArgumentException($"'{email}' cannot be null or empty.", parameterName);

    if (!EmailRegex().IsMatch(email))
      throw new ArgumentException($"'{email}' email format incorrect.", parameterName);
  }

  public static void CharAndDigit(string name, string parameterName)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException($"'{name}' cannot be null or empty.", parameterName);

    if (!CharAndDigitRegex().IsMatch(name))
      throw new ArgumentException($"'{name}' only char and digit support.", parameterName);
  }

  public static void Name(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException($"'{name}' cannot be null or empty.", name);

    if (!NameRegex().IsMatch(name))
      throw new ArgumentException($"'{name}' format incorrect.", name);
  }

  public static void IsNullOrEmpty(string value, string paramter)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException($"'{value}' cannot be null or empty.", paramter);
    }
  }

  public static void NotMinus(int value, string parameterName)
  {
    if (value < 0)
    {
      throw new ArgumentException($"'{parameterName}' cannot be zero.", parameterName);
    }
  }

  public static void GuidEmpty(Guid value, string parameterName)
  {
    if (value == System.Guid.Empty)
    {
      throw new ArgumentException($"'{parameterName}' cannot be empty.", parameterName);
    }
  }



  public static void Digit(string value, string param)
  {
    if (!DigitRegex().IsMatch(value))
      throw new ArgumentException($"'{value}' must be only digit.", param);
  }

  public static bool ValidateTenants(string Tenants, int LocationId)
  {
    if (string.IsNullOrWhiteSpace(Tenants))
      return false;

    var arr = Tenants.Split(",").Select(x => int.Parse(x)).ToList();

    return arr.Contains(LocationId) || LocationId == 0;
  }

  public static void ValidateDeviceType(string Type)
  {
    if (string.IsNullOrWhiteSpace(Type))
      throw new ArgumentException("Device Type is empty");

    bool isMatch = Enum.TryParse<DeviceType>(Type, true, out _);
    if (!isMatch)
      throw new ArgumentException("Device Type is invalid");
  }

  public static void ValidateDateTime(string value, DateTime dateTime)
  {
    if (value == default)
      throw new ArgumentException($"'{value}' is invalid.", dateTime.ToString());
  }

  public static void ValidateActiveTime(DateTime active, DateTime expire)
  {
    if (active > expire)
      throw new ArgumentException($"Active time must be lower than expire time.");

    if (active == expire)
      throw new ArgumentException($"Active time must be lower than expire time.");

  }

  public static string Password(
    string password,
    int len,
    bool digit,
    bool symbol,
    bool upper,
    bool lower
    )
  {
    if (string.IsNullOrWhiteSpace(password))
      return "Empty";
    // Length
    if (password.Length < len)
      return $"Length must greater than {len}";

    if (upper && !password.Any(char.IsUpper))
      return $"Uppercase required";

    if (lower && !password.Any(char.IsLower))
      return $"Lowercase required";

    if (digit && !password.Any(char.IsDigit))
      return $"Digit required";

    if (symbol && !password.Any(c => !char.IsLetterOrDigit(c)))
      return $"Symbol required";

    return string.Empty;

  }



}
