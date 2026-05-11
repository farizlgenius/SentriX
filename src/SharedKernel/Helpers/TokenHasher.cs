using System;
using System.Security.Cryptography;
using System.Text;

namespace SharedKernel.Helpers;

public static class TokenHasher
{
  public static string Hash(string token)
  {
    var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
    return Convert.ToBase64String(hash);
  }

  // public static string Hash(string raw)
  // {
  //   using var sha = SHA256.Create();
  //   var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
  //   return Convert.ToHexString(bytes).ToLowerInvariant();
  // }

  public static bool AreEqual(string hash1Base64, string hash2Base64)
  {
    var hash1 = Convert.FromBase64String(hash1Base64);
    var hash2 = Convert.FromBase64String(hash2Base64);

    return CryptographicOperations.FixedTimeEquals(hash1, hash2);
  }
}
