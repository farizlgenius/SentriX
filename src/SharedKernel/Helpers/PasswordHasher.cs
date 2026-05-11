using System;
using System.Security.Cryptography;
using System.Text;

namespace SharedKernel.Helpers;

public static class PasswordHasher
{
  private const int SaltSize = 16;        // 128-bit salt
  private const int KeySize = 32;         // 256-bit subkey
  private const int Iterations = 100_000; // OWASP recommendation

  public static string HashPassword(string password)
  {
    if (string.IsNullOrWhiteSpace(password))
      throw new ArgumentException("Password cannot be empty");

    byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

    byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
        password: Encoding.UTF8.GetBytes(password),
        salt: salt,
        iterations: Iterations,
        hashAlgorithm: HashAlgorithmName.SHA256,
        outputLength: KeySize
    );

    return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
  }

  public static bool VerifyPassword(string password, string storedHash)
  {
    if (string.IsNullOrWhiteSpace(storedHash))
      return false;

    var parts = storedHash.Split('.', 3);
    if (parts.Length != 3) return false;

    int iterations = int.Parse(parts[0]);
    byte[] salt = Convert.FromBase64String(parts[1]);
    byte[] expectedHash = Convert.FromBase64String(parts[2]);

    byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
        password: Encoding.UTF8.GetBytes(password),
        salt: salt,
        iterations: iterations,
        hashAlgorithm: HashAlgorithmName.SHA256,
        outputLength: expectedHash.Length
    );

    return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
  }

  public static bool NeedsRehash(string storedHash)
  {
    var parts = storedHash.Split('.', 3);
    if (parts.Length != 3) return true;

    int iterations = int.Parse(parts[0]);
    return iterations < Iterations;
  }
}