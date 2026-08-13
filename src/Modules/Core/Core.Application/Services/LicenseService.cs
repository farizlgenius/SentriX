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
      IMachine mac,
      IHttpClient client,
      IStorage storage,
      ILicenseSetting licenseSetting
) : ILicense
{
      public async Task<bool> CheckLicenseAsync(CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> GenerateDemoAsync(CreateDemoLicenseDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> GenerateLicenseAsync(CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<MachineIdDto> GetMachineIdAsync(CancellationToken ct = default)
      {
            return new MachineIdDto(mac.Get());
      }

      private async Task<Handshake> InitHandshakeAsync()
      {

            // Step 1 : Generate Dh and Load Signer from
            var appDh = EncryptHelper.CreateDh();
            var appDhPublic = appDh.ExportSubjectPublicKeyInfo();

            // Step 2 : Get Public Sign from file
            var appSingPublic = await storage.ReadByteKeyAsync("pub_sign.key");

            // Step 3 : Get Private Sign from file
            var appSignPrivate = await storage.ReadByteKeyAsync("pri_sign.key");


            // Step 4 : Sign ECDH public key with Sign private key
            var signData = appDhPublic.Concat(appSingPublic).ToArray();
            var signature = EncryptHelper.SignData(EncryptHelper.LoadSignerPrivateKey(appSignPrivate), signData);

            // Step 5 : Exchange Key with license server

            var body = new ExchageReq(
                  Guid.NewGuid(),
                  Convert.ToBase64String(appDhPublic),
                  Convert.ToBase64String(appSingPublic),
                  Convert.ToBase64String(signature)
            );

            var res = await client.SendAsync<ExchageReq, BaseResponse<ExchangeRes>>(
                  HttpMethod.Post,
                  licenseSetting.Uri,
                  licenseSetting.Endpoint.Exchange,
                  body);

            if (res.Data is null)
                  throw new Exception("Http response error");

            // Step 6 : Calculate License server response
            var serverDhPublic = Convert.FromBase64String(res.Data.DhPub);
            var serverSignPublic = Convert.FromBase64String(res.Data.SignPub);
            var serverSignature = Convert.FromBase64String(res.Data.Signature);

            var licVerifyData = serverDhPublic.Concat(serverSignPublic).ToArray();
            if (!EncryptHelper.VerifyData(licVerifyData, serverSignature, serverSignPublic))
                  throw new Exception("Exchange key verify data fail");

            // Step 7 : Derive Shared Key
            var sharedKey = EncryptHelper.DeriveSecretKey(appDh, serverDhPublic);
            var aesKey = EncryptHelper.DeriveAesKey(sharedKey, licenseSetting.Secret);

            return new Handshake(res.Data.SessionId, aesKey);
      }
}