using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Command;

public class BaseCommand : IBaseCommand
{
      protected IHttpClient Client { get; }
      protected IAmicoSetting Setting { get; }

      protected BaseCommand(IHttpClient client, IAmicoSetting setting)
      {
            Client = client;
            Setting = setting;
      }

      public async Task<CheckSessionResponse> CheckSession(string ip, string session)
      {
            return await Client.SendAsync<CheckSessionResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.CHECK_SESSION
            ) ?? new CheckSessionResponse();
      }

      public async Task<LoginResponse> LoginAsync(string ip)
      {
            return await Client.SendAsync<LoginRequest, LoginResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.LOGIN,
                  new LoginRequest(Setting.Login, Setting.Password)
            ) ?? new LoginResponse();
      }

      public async Task<bool> LogoutAsync(string ip, string session)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };
            var res = await Client.SendAsync<object>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.LOGIN,
                  queryParams: queryParams
            );

            return true;
      }
}