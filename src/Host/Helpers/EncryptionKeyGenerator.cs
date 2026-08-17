using System.Security.Cryptography;

namespace Host.Helpers;

public static class EncryptionKeyGenerator

{

  public static (byte[] PrivateKey, byte[] PublicKey)

      GenerateEcdh()

  {

    using var key =

        ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);

    return (

        key.ExportPkcs8PrivateKey(),

        key.ExportSubjectPublicKeyInfo());

  }

}

///
/// 
/// Backend ECDSA
/// private → request signing
/// public  → License Server
/// Backend ECDH
/// private → license decryption
/// public  → License Server
/// 
/// 