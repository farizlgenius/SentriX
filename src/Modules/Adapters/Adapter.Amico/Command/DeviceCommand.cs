using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Command;

public sealed class DeviceCommand(IHttpClient client,IAmicoSetting setting) : BaseCommand(client,setting),IDeviceCommand
{
      public async Task ChangeLogin(string ip,string login, string password,string session)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            await Client.SendAsync<LoginRequest,object>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip,Setting.Secure),
                  Endpoint.CHANGE_LOGIN,
                  new LoginRequest(
                        login,
                        password
                  ),
                  queryParams:queryParams
            );
      }

      public  async Task<DeviceInfoResponse> DeviceInfoAsync(string ip,string session)
      {
            var headers = new Dictionary<string, string>
            {
                  { "cookie", $"session={session}" }
            };

            return await Client.SendAsync<DeviceInfoResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip,Setting.Secure),
                  Endpoint.DEVICE_INFO,
                  headers
            ) ?? new DeviceInfoResponse();
      }

      


  

   
}