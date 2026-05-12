using System;

namespace SharedKernel.Helpers;

public sealed class ValidationHelper
{
  public static void ValidateNotNullOrEmpty(string value, string parameterName)
  {
    if (string.IsNullOrEmpty(value))
    {
      throw new ArgumentException($"'{parameterName}' cannot be null or empty.", parameterName);
    }
  }

  public static void ValidateNotMinus(int value, string parameterName)
  {
    if (value < 0)
    {
      throw new ArgumentException($"'{parameterName}' cannot be zero.", parameterName);
    }
  }

  public static bool ValidateTenants(string Tenants,int LocationId)
      {
            if(string.IsNullOrWhiteSpace(Tenants))
                  return false;

            var arr = Tenants.Split(",").Select(x => int.Parse(x)).ToList();

            return arr.Contains(LocationId);
      }

}
