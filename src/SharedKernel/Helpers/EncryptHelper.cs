using System.Security.Cryptography;
using System.Text;

namespace SharedKernel.Helpers;

public sealed class EncryptHelper()
{
      
       #region Hash Algrithm
        public static string Hash(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        public static bool CompareHash(string rawInput, string storedHash)
        {
            var hashBytes1 = Convert.FromHexString(Hash(rawInput));
            var hashBytes2 = Convert.FromHexString(storedHash);

            return CryptographicOperations.FixedTimeEquals(hashBytes1, hashBytes2);
        }
      //   public static string HashPassword(string password)
      //   {
      //       // generate a random salt
      //       byte[] salt = RandomNumberGenerator.GetBytes(16);

      //       // derive a 32-byte hash with PBKDF2 (100,000 iterations recommended)
      //       var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
      //       byte[] hash = pbkdf2.GetBytes(32);

      //       // combine salt + hash into one string (Base64)
      //       byte[] result = new byte[salt.Length + hash.Length];
      //       Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
      //       Buffer.BlockCopy(hash, 0, result, salt.Length, hash.Length);

      //       return Convert.ToBase64String(result);
      //   }

      //   public static bool VerifyPassword(string password, string storedHash)
      //   {
      //       byte[] storedBytes = Convert.FromBase64String(storedHash);

      //       byte[] salt = new byte[16];
      //       Buffer.BlockCopy(storedBytes, 0, salt, 0, salt.Length);

      //       byte[] storedHashBytes = new byte[32];
      //       Buffer.BlockCopy(storedBytes, salt.Length, storedHashBytes, 0, 32);

      //       var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
      //       byte[] computedHash = pbkdf2.GetBytes(32);

      //       // constant-time comparison to prevent timing attacks
      //       return CryptographicOperations.FixedTimeEquals(computedHash, storedHashBytes);
      //   }

        #endregion


      #region License Encrypt Helper

      /// <summary>
      /// Generates ECDH key pair for key exchange.
      /// </summary>
      /// <returns>The ECDH key pair.</returns>
      public static ECDiffieHellman CreateDh() => ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);

      /// <summary>
      /// Generate ECDSA key pair for signing.
      /// </summary>
      /// <returns>The ECDSA key pair.</returns
      public static ECDsa CreateSigner() => ECDsa.Create(ECCurve.NamedCurves.nistP256);

      /// <summary>
      /// Exports the public key from the ECDH key pair.
      /// </summary>
      /// <param name="dh">The ECDH key pair.</param>
      /// <returns>The public key bytes.</returns>
      public static byte[] ExportDhPublicKey(ECDiffieHellman dh) => dh.ExportSubjectPublicKeyInfo();

      /// <summary>
      /// Exports the public key from the ECDSA key pair.
      /// </summary>
      /// <param name="signer">The ECDSA key pair.</param>
      /// <returns>The public key bytes.</returns>
      public static byte[] ExportSignerPublicKey(ECDsa signer) => signer.ExportSubjectPublicKeyInfo();

      /// <summary>
      /// Imports the ECDSA public key.
      /// </summary>
      /// <param name="privateKey">The private key bytes.</param>
      /// <returns>The ECDSA key pair.</returns>
      public static ECDsa LoadSignerPrivateKey(byte[] privateKey)
      {
            var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            ecdsa.ImportPkcs8PrivateKey(privateKey, out _);
            return ecdsa;
      }

      /// <summary>
      /// Imports the ECDSA public key.
      /// </summary>
      /// <param name="publicKey">The public key bytes.</param>
      /// <returns>The ECDSA key pair.</returns>
      public static ECDsa LoadVerifierPublicKey(byte[] publicKey)
      {
            var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
            return ecdsa;
      }


      /// <summary>
      /// Derives the shared secret key using ECDH key exchange.
      /// </summary>
      /// <param name="dh">The ECDiffieHellman instance.</param>
      /// <param name="otherPub">The peer public key bytes.</param>
      /// <returns>The derived secret key bytes.</returns>
      public static byte[] DeriveSecretKey(ECDiffieHellman dh, byte[] otherPub)
      {
            using var other = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
            other.ImportSubjectPublicKeyInfo(otherPub, out _);
            return dh.DeriveKeyMaterial(other.PublicKey);
      }

      /// <summary>
      /// Signs the data using ECDSA.
      /// </summary>
      /// <param name="signer">The ECDsa instance.</param>
      /// <param name="data">The data to sign.</param>
      /// <returns>The signature bytes.</returns>
      public static byte[] SignData(ECDsa signer, byte[] data)
      {
            return signer.SignData(data, HashAlgorithmName.SHA256);
      }

      /// <summary>
      /// Verifies the data signature using ECDSA.
      /// </summary>
      /// <param name="data">The data to verify.</param>
      /// <param name="sign">The signature bytes.</param>
      /// <param name="pub">The public key bytes.</param>
      /// <returns>True if the signature is valid, false otherwise.</returns>
      public static bool VerifyData(byte[] data, byte[] sign, byte[] pub)
      {
            using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            ecdsa.ImportSubjectPublicKeyInfo(pub, out _);
            return ecdsa.VerifyData(data, sign, HashAlgorithmName.SHA256);
      }

      /// <summary>
      /// Encrypts the data using AES-GCM.
      /// </summary>
      /// <param name="key">The AES key.</param>
      /// <param name="data">The data to encrypt.</param>
      /// <returns>The encrypted data.</returns>
      public static byte[] EncryptAes(byte[] key, byte[] data)
      {
            var iv = RandomNumberGenerator.GetBytes(12);
            var cipher = new byte[data.Length];
            var tag = new byte[16];

            using var aes = new AesGcm(key, tagSizeInBytes: 16);
            aes.Encrypt(iv, data, cipher, tag);

            return iv.Concat(tag).Concat(cipher).ToArray();
      }

      /// <summary>
      /// Decrypts the data using AES-GCM.
      /// </summary>
      /// <param name="key">The AES key.</param>
      /// <param name="encryptedData">The encrypted data.</param>
      /// <returns>The decrypted data.</returns>
      public static byte[] DecryptAes(byte[] key, byte[] encryptedData)
      {
            var iv = encryptedData[..12];
            var tag = encryptedData[12..28];
            var cipher = encryptedData[28..];

            var plain = new byte[cipher.Length];

            using var aes = new AesGcm(key, tagSizeInBytes: 16);
            aes.Decrypt(iv, cipher, tag, plain);

            return plain;
      }

      /// <summary>
      /// Derives AES key from shared secret using HKDF.
      /// </summary>
      /// <param name="sharedSecret"></param>
      /// <returns></returns>
      public static byte[] DeriveAesKey(byte[] sharedSecret, string Info)
      {
            return HKDF.DeriveKey(
                HashAlgorithmName.SHA256,
                sharedSecret,
                32,
                salt: null,
                info: Encoding.UTF8.GetBytes(Info)
            );
      }

      /// <summary>
      /// Parses the payload into license and signature.
      /// </summary>
      /// <param name="payload"></param>
      /// <returns></returns>
      public static (byte[] license, byte[] signature) ParsePayload(byte[] payload)
      {
            using var ms = new MemoryStream(payload);
            using var br = new BinaryReader(ms);

            int licLen = br.ReadInt32();
            byte[] license = br.ReadBytes(licLen);

            int sigLen = br.ReadInt32();
            byte[] sig = br.ReadBytes(sigLen);

            return (license, sig);
      }

      #endregion
}