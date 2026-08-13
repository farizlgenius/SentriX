using Core.Application.Interfaces;
using Core.Contract.DTOs.License;
using Core.Contract.Interfaces;
using SharedKernel.Helpers;

namespace Core.Application.Services;

public sealed class LicenseService(
      IMachine mac
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

      private async Task<ResponseDto<HandshakeResult>> InitHandshakeAsync()
      {

            // Step 1 : Generate Dh and Load Signer from
            var appDh = EncryptHelper.CreateDh();
            var appDhPublic = appDh.ExportSubjectPublicKeyInfo();

            // Step 2 : Get Public Sign from file
            string pubSignFile = Path.Combine(Path.Combine(AppContext.BaseDirectory, "data"), "pub_sign.key");
            if (!File.Exists(pubSignFile))
            {
                  return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.LICENSE_ERR, "Sign public key file not found");
            }

            if (new FileInfo(pubSignFile).Length <= 0)
            {
                  return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.LICENSE_ERR, "Sign public key empty");
            }

            // Step 3 : Get Private Sign from file
            string priSignFile = Path.Combine(Path.Combine(AppContext.BaseDirectory, "data"), "pri_sign.key");
            if (!File.Exists(priSignFile))
            {
                  return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.LICENSE_ERR, "Sign private key file not found");
            }

            if (new FileInfo(pubSignFile).Length <= 0)
            {
                  return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.LICENSE_ERR, "Sign private key empty");
            }

            // Step 4 : Sign ECDH public key with Sign private key
            var appSingPublic = await File.ReadAllBytesAsync(pubSignFile);
            var appSignPrivate = await File.ReadAllBytesAsync(priSignFile);
            var signData = appDhPublic.Concat(appSingPublic).ToArray();
            var signature = EncryptHelper.SignData(EncryptHelper.LoadSignerPrivateKey(appSignPrivate), signData);

            // Step 5 : Exchange Key with license server

            var body = new ExchangeRequest(Guid.NewGuid().ToString(), Convert.ToBase64String(appDhPublic), Convert.ToBase64String(appSingPublic), Convert.ToBase64String(signature));

            var response = await http.ExchangeAsync(body);

            if (response.payload is null) return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.LICENSE_ERR, response.message);

            // Step 6 : Calculate License server response
            var serverDhPublic = Convert.FromBase64String(response.payload.dhPub);
            var serverSignPublic = Convert.FromBase64String(response.payload.signPub);
            var serverSignature = Convert.FromBase64String(response.payload.signature);

            var licVerifyData = serverDhPublic.Concat(serverSignPublic).ToArray();
            if (!EncryptHelper.VerifyData(licVerifyData, serverSignature, serverSignPublic))
            {
                  // Verify fail
                  return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.LICENSE_ERR, "Exchange key verify data fail");
            }

            // Step 7 : Derive Shared Key
            var sharedKey = EncryptHelper.DeriveSecretKey(appDh, serverDhPublic);
            var aesKey = EncryptHelper.DeriveAesKey(sharedKey, settings.LicenseSettings.Secret);

            return ResponseHelper.SuccessBuilder(new HandshakeResult(response.payload.sessionId, aesKey));
      }
}