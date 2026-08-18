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
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Interfaces;
using Storage.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class LicenseService(
      ILicenseRepository repo,
      IHttpClient http,
      ILicenseSetting setting,
      IStorage storage
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

      public async Task<bool> RequestDemoAsync(DemoLicenseDto dto, CancellationToken ct = default)
      {
            // Send request to generte demo
            // string BackendId,
            // string KeyId,
            // long Timestamp,
            // string RequestId,
            // string BodyHash,
            // string Signature
            ValidationHelper.IsNullOrEmpty(dto.MachineId, nameof(dto.MachineId));
            ValidationHelper.IsNullOrEmpty(dto.Customer, nameof(dto.Customer));
            ValidationHelper.IsNullOrEmpty(dto.EndUser, nameof(dto.EndUser));

            var body = new DemoHttpRequest(
                  dto.Customer,
                  dto.EndUser,
                  dto.MachineId,
                  "Sentrix"
            );

            var requestId = Guid.NewGuid().ToString();

            var bodyJson = JsonHelper.Serialize(body);

            var bodyByte = Encoding.UTF8.GetBytes(bodyJson);

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var backendId = $"{timestamp}-${dto.Customer}/${dto.EndUser}";

            var canonical = RequestSigner.BuildCanonicalRequest(
                  HttpMethod.Post.Method,
                  setting.Endpoint.Demo,
                  requestId,
                  timestamp,
                  bodyByte
            );

            // Get private key
            var pri = await storage.ReadByteKeyAsync("pri.key");

            string signature = RequestSigner.Sign(canonical, pri);

            var headers = new Dictionary<string, string>
            {
                  {"X-Backend-Id",backendId},
                  {"X-Timestamp",timestamp.ToString()},
                  {"X-Request-Id",requestId},
                  {"X-Signature",signature}
            };

            var res = http.SendAsync<DemoHttpRequest, DemoHttpResponse>(
                  HttpMethod.Post,
                  setting.Uri,
                  setting.Endpoint.Demo,
                  body,
                  headers,
                  ct: ct
            );


      }
}