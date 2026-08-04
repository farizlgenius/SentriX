using System.Runtime.Serialization;
using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Enums;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;
using Door.Contract.Queries;
using SharedKernel.Messaging;
using Time.Contract.Queries;

namespace Adapter.Amico.Command;

public sealed class DeviceCommand(IHttpClient client,IAmicoSetting setting,IMessageBus bus,IAmicoRepository repo) : BaseCommand(client,setting,repo),IDeviceCommand
{

      public async Task ChangeLogin(string ip,string session)
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
                        Setting.Login,
                        Setting.Password
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

      public async Task<bool> VerifyDeviceComponentAsync(string ip, string session,int location_id)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            // TimZones => Timezone

            var res = await Client.SendAsync<LoadObjectRequest,LoadObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip,Setting.Secure),
                  Endpoint.LOAD_OBJECT,
                  new LoadObjectRequest(
                        ObjectConstant.TimeZone,
                        ["id"]
                  ),
                  queryParams:queryParams
            );

            var v = await bus.QueryAsync(new TimeZoneCountByLocationIdQuery(location_id));

            if(res is null)
                  return false;

            if(res.time_zones.Count() != v)
                  return false;

            // Holiday

            res = await Client.SendAsync<LoadObjectRequest,LoadObjectResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip,Setting.Secure),
                  Endpoint.LOAD_OBJECT,
                  new LoadObjectRequest(
                        ObjectConstant.Holiday,
                        ["id"]
                  ),
                  queryParams:queryParams
            );

            
            v = await bus.QueryAsync(new TimeZoneCountByLocationIdQuery(location_id));

            if(res is null)
                  return false;

            if(res.holidays.Count() != v)
                  return false;

            return true;
      }
}