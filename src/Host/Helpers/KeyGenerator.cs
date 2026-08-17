using System.Security.Cryptography;

namespace Host.Helpers;

public static class KeyGenerator

{

  public static (byte[] PrivateKey, byte[] PublicKey)

      GenerateEcdsa()

  {

    using var key =

        ECDsa.Create(ECCurve.NamedCurves.nistP256);

    return (

        key.ExportPkcs8PrivateKey(),

        key.ExportSubjectPublicKeyInfo());

  }

}