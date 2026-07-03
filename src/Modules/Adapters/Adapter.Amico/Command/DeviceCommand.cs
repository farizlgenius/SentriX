using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Command;

public sealed class DeviceCommand(IHttpClient client,IAmicoSetting setting) : IDeviceCommand
{
      public async Task<CheckSessionResponse> CheckSession(string ip, string session)
      {
            return await client.SendAsync<CheckSessionResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip,setting.Secure),
                  Endpoint.CHECK_SESSION
            ) ?? new CheckSessionResponse();
      }
      
      public  async Task<DeviceInfoResponse> DeviceInfoAsync(string ip,string session)
      {
            var headers = new Dictionary<string, string>
            {
                  { "cookie", $"session={session}" }
            };

            return await client.SendAsync<DeviceInfoResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip,setting.Secure),
                  Endpoint.DEVICE_INFO,
                  headers
            ) ?? new DeviceInfoResponse();
      }


      public async Task<LoginResponse> LoginAsync(string ip, string login, string password)
      {
            return await client.SendAsync<LoginRequest,LoginResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip,setting.Secure),
                  Endpoint.LOGIN,
                  new LoginRequest(login,password)
            ) ?? new LoginResponse();
      }

      public async Task<bool> LogoutAsync(string ip,string session)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };
            var res = await client.SendAsync<object>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip,setting.Secure),
                  Endpoint.LOGIN,
                  queryParams:queryParams
            );

            return true;
      }
}