using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Application.Helpers;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Application.Models.Requests;
using Core.Application.Models.Responses;
using Core.Contract.DTOs.License;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Interfaces;
using Storage.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class LicenseService(
      ILicenseRepository repo,
      IHttpClient http,
      ILicenseSetting setting,
      IStorage storage,
      IMachine machine
      ) : ILicense
{
    public Task<bool> ActivateAsync(ActivateDto dto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> DownloadAsync(DownloadLicenseDto dto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetMachineIdAsync(CancellationToken ct = default)
    {
        return machine.Get();
    }

    public async Task<object> RequestDemoAsync(

  DemoLicenseDto dto,

  CancellationToken ct = default)

    {

        ValidationHelper.IsNullOrEmpty(

            dto.MachineId,

            nameof(dto.MachineId));

        ValidationHelper.IsNullOrEmpty(

            dto.Customer,

            nameof(dto.Customer));

        ValidationHelper.IsNullOrEmpty(

            dto.EndUser,

            nameof(dto.EndUser));

        // --------------------------------------------------

        // 1. Request metadata

        // --------------------------------------------------

        var requestId =

            Guid.NewGuid().ToString("N");

        var timestamp =

            DateTimeOffset.UtcNow

                .ToUnixTimeSeconds();

        var backendId =

            $"{timestamp}-{dto.Customer}/{dto.EndUser}";

        // --------------------------------------------------

        // 2. Create request object

        // --------------------------------------------------

        var body = new DemoHttpRequest(

            backendId,

            dto.Customer,

            dto.EndUser,

            dto.MachineId,

            "Sentrix",

            Convert.ToBase64String(

                await storage.ReadByteKeyAsync(

                    "pub.key")),

            Convert.ToBase64String(

                await storage.ReadByteKeyAsync(

                    "enc_pub.key"))

        );

        // --------------------------------------------------

        // 3. Serialize ONCE

        // --------------------------------------------------

        // var bodyJson = JsonSerializer.Serialize(body);

        var bodyJson = JsonHelper.Serialize(body);

        // --------------------------------------------------

        // 4. Convert to bytes ONCE

        // --------------------------------------------------

        var bodyBytes =

            Encoding.UTF8.GetBytes(bodyJson);

        // var bodyBytes = JsonSerializer.SerializeToUtf8Bytes(body);

        // --------------------------------------------------

        // 5. Calculate Body Hash

        // --------------------------------------------------

        var bodyHash =

            RequestSigner.ComputeBodyHash(

                bodyBytes);

        // --------------------------------------------------

        // 6. Build canonical request

        // --------------------------------------------------

        var canonical =

            RequestSigner.BuildCanonicalRequest(

                HttpMethod.Post.Method,

                setting.Endpoint.Demo,

                requestId,

                timestamp,

                bodyBytes

            );

        Console.WriteLine("========== BACKEND ==========");

        Console.WriteLine(

            $"Body: {Encoding.UTF8.GetString(bodyBytes)}");

        Console.WriteLine(

            $"Body Hash: {bodyHash}");

        Console.WriteLine(

            $"Request ID: {requestId}");

        Console.WriteLine(

            $"Timestamp: {timestamp}");

        Console.WriteLine(

            $"Path: {setting.Endpoint.Demo}");

        Console.WriteLine(

            $"Canonical:");

        Console.WriteLine(canonical);

        // --------------------------------------------------

        // 7. Get ECDSA private key

        // --------------------------------------------------

        var pri =

            await storage.ReadByteKeyAsync(

                "pri.key");

        // --------------------------------------------------

        // 8. Sign canonical request

        // --------------------------------------------------

        var signature =

            RequestSigner.Sign(

                canonical,

                pri);

        Console.WriteLine(

            $"Signature: {signature}");

        // --------------------------------------------------

        // 9. Create headers

        // --------------------------------------------------

        var headers =

            new Dictionary<string, string>

            {

            {

                "X-Backend-Id",

                backendId

            },

            {

                "X-Timestamp",

                timestamp.ToString()

            },

            {

                "X-Request-Id",

                requestId

            },

            {

                "X-Body-Hash",

                bodyHash

            },

            {

                "X-Signature",

                signature

            },

            {

                "X-Public",

                Convert.ToBase64String(

                    await storage.ReadByteKeyAsync(

                        "pub.key"))

            }

            };

        // --------------------------------------------------

        // 10. SEND THE EXACT SAME bodyJson

        // --------------------------------------------------

        var res =

            await http.SendAsync<

                DemoHttpRequest,

                BaseResponse<DemoHttpResponse>>(

                    HttpMethod.Post,

                    setting.Uri,

                    setting.Endpoint.Demo,

                    body,

                    headers,

                    ct: ct);

        // --------------------------------------------------

        // 11. ECDH

        // --------------------------------------------------

        using var myEcdh =

            ECDiffieHellman.Create();

        myEcdh.ImportPkcs8PrivateKey(

            await storage.ReadByteKeyAsync(

                "enc_pri.key"),

            out _);

        using var otherEcdh =

            ECDiffieHellman.Create();

        otherEcdh.ImportSubjectPublicKeyInfo(

            Convert.FromBase64String(

                res.Data.EcdsaPublicKey),

            out _);

        var sharedSecret =

            myEcdh.DeriveKeyMaterial(

                otherEcdh.PublicKey);

        Console.WriteLine(

            $"Shared Secret: " +

            Convert.ToBase64String(

                sharedSecret));

        // --------------------------------------------------

        // 12. Decrypt response

        // --------------------------------------------------

        var aes =

            new AesSecretProtector(

                Convert.ToBase64String(

                    sharedSecret));

        var jsonBody =

            aes.Unprotect(

                res.Data.CipherText);

        if (jsonBody is null)

        {

            throw new Exception(

                "Decrypt data failed.");

        }

        // --------------------------------------------------

        // 13. Verify License Server signature

        // --------------------------------------------------

        using var ecdsa =

            ECDsa.Create();

        ecdsa.ImportSubjectPublicKeyInfo(

            Convert.FromBase64String(

                res.Data.EcdsaPublicKey),

            out _);

        var verify =

            ecdsa.VerifyData(

                Encoding.UTF8.GetBytes(

                    jsonBody),

                Convert.FromBase64String(

                    res.Data.Signature),

                HashAlgorithmName.SHA256);

        if (!verify)

        {

            Console.WriteLine(

                "========== RESPONSE SIGNATURE FAILED ==========");

            Console.WriteLine(

                $"JSON Body: {jsonBody}");

            Console.WriteLine(

                $"Signature: {res.Data.Signature}");

            throw new Exception(

                "Verify signature failed.");

        }

        // --------------------------------------------------

        // 14. Deserialize license

        // --------------------------------------------------

        var license =

            JsonHelper.Deserialize<

                LicensePayload>(

                    jsonBody);

        return license;

    }
}