using System.Globalization;
using System.Security.Cryptography;
using System.Text;

public static class RequestSigner
{
  public static string ComputeBodyHash(byte[] bodyBytes)
  {
    var hash = SHA256.HashData(bodyBytes);

    return Convert.ToHexString(hash)
        .ToLowerInvariant();
  }

  public static string BuildCanonicalRequest(
      string method,
      string path,
      string requestId,
      long timestamp,
      byte[] bodyBytes)
  {
    var bodyHash = ComputeBodyHash(bodyBytes);

    return string.Join(
        "\n",
        method.ToUpperInvariant(),
        path,
        requestId,
        timestamp.ToString(
            CultureInfo.InvariantCulture),
        bodyHash
    );
  }

  public static string Sign(
      string canonical,
      byte[] privateKey)
  {
    using var ecdsa = ECDsa.Create();

    ecdsa.ImportPkcs8PrivateKey(
        privateKey,
        out _);

    var data =
        Encoding.UTF8.GetBytes(canonical);

    var signature =
        ecdsa.SignData(
            data,
            HashAlgorithmName.SHA256);

    return Convert.ToBase64String(signature);
  }
}