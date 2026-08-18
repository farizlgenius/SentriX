using System.Security.Cryptography;
using System.Text;

namespace Core.Application.Helpers;

public static class RequestSigner
{
  public static string BuildCanonicalRequest(
        string method,
        string path,
        string requestId,
        long timestamp,
        byte[] body)
  {
    var bodyHash = Convert.ToHexString(SHA256.HashData(body)).ToLowerInvariant();

    return string.Join(
        "\n",
        method.ToUpperInvariant(),
        path,
        requestId,
        timestamp,
        bodyHash);
  }

  public static string Sign(
      string canonical,
      byte[] privateKey)
  {

    using var ecdsa = ECDsa.Create();

    ecdsa.ImportPkcs8PrivateKey(
        privateKey,
        out _);

    var data = Encoding.UTF8.GetBytes(canonical);

    var signature =
        ecdsa.SignData(
            data,
            HashAlgorithmName.SHA256);

    return Convert.ToBase64String(signature);

  }
}