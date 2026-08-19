using System.Security.Cryptography;
using System.Text;

namespace Core.Application.Helpers;

public sealed class AesSecretProtector
{
  private readonly byte[] _masterKey;

  public AesSecretProtector(string key)
  {

    _masterKey = Convert.FromBase64String(key);

    if (_masterKey.Length != 32)
      throw new ArgumentException("Master key must be 256 bits.");

  }

  public string Protect(string plaintext)
  {

    byte[] nonce = RandomNumberGenerator.GetBytes(12);
    byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
    byte[] ciphertext = new byte[plaintextBytes.Length];
    byte[] tag = new byte[16];
    using var aes = new AesGcm(_masterKey, 16);

    aes.Encrypt(
        nonce,
        plaintextBytes,
        ciphertext,
        tag);

    return string.Join(
        ".",
        Convert.ToBase64String(nonce),
        Convert.ToBase64String(tag),
        Convert.ToBase64String(ciphertext));

  }

  public string Unprotect(string protectedText)
  {
    string[] parts = protectedText.Split('.');
    byte[] nonce = Convert.FromBase64String(parts[0]);
    byte[] tag = Convert.FromBase64String(parts[1]);
    byte[] ciphertext = Convert.FromBase64String(parts[2]);
    byte[] plaintext = new byte[ciphertext.Length];
    using var aes = new AesGcm(_masterKey, 16);
    aes.Decrypt(
        nonce,
        ciphertext,
        tag,
        plaintext);
    return Encoding.UTF8.GetString(plaintext);
  }
}