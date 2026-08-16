using System.Security.Cryptography;
using System.Text;

namespace SharedKernel.Helpers;

public static class LicenseHelper
{
  public static string BuildCanonicalMessage(
    string method,
    string path,
    string requestId,
    long timestamp,
    byte[] body)

  {

    var bodyHash =

        SHA256.HashData(body);

    var bodyHashBase64 =

        Convert.ToBase64String(bodyHash);

    return string.Join(

        "\n",

        method.ToUpperInvariant(),

        path,

        requestId,

        timestamp,

        bodyHashBase64);

  }

  public static string Sign(

        byte[] privateKey,

        string canonicalMessage)

  {

    var data = Encoding.UTF8.GetBytes(canonicalMessage);

    using (ECDsa ecdsa = ECDsa.Create())
    {
      ecdsa.ImportPkcs8PrivateKey(privateKey, out _);
      var signature = ecdsa.SignData(data, HashAlgorithmName.SHA256);

      return Convert.ToBase64String(signature);
    }

  }

  public static bool Verify(

      byte[] publicKey,

      string canonicalMessage,

      string signature)

  {

    var data = Encoding.UTF8.GetBytes(canonicalMessage);

    var signatureBytes = Convert.FromBase64String(signature);

    using (ECDsa ecdsa = ECDsa.Create())
    {
      ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
      return ecdsa.VerifyData(data, signatureBytes, HashAlgorithmName.SHA256);
    }

  }
}