using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Request;
using Adapter.Amico.Model.Response;
using SharedKernel.Interfaces;

namespace Adapter.Amico.Command;

public class BaseCommand : IBaseCommand
{
      protected IHttpClient Client { get; }
      protected IAmicoSetting Setting { get; }
      private IAmicoRepository _repo { get; }

      protected BaseCommand(IHttpClient client, IAmicoSetting setting, IAmicoRepository repo)
      {
            Client = client;
            Setting = setting;
            _repo = repo;
      }

      private async Task<CheckSessionResponse> CheckSession(string ip, string session)
      {
            return await Client.SendAsync<CheckSessionResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.CHECK_SESSION
            ) ?? new CheckSessionResponse();
      }

      public async Task<LoginResponse> LoginAsync(string ip, bool? isFirst)
      {
            var login = "";
            var password = "";
            if (isFirst ?? false)
            {
                  login = Setting.DefaultLogin;
                  password = Setting.DefaultPassword;
            }
            else
            {
                  login = Setting.Login;
                  password = Setting.Password;
            }
            return await Client.SendAsync<LoginRequest, LoginResponse>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.LOGIN,
                  new LoginRequest(login, password)
            ) ?? new LoginResponse();
      }

      private async Task<bool> LogoutAsync(string ip, string session)
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

      public async Task<string> CheckSessionAsync(string mac)
      {
            // var amico = await _repo.GetAmicoByMacAsync(mac);
            // var session = amico.session;

            // var res = await CheckSession(amico.ip, amico.session);

            // if (!res.SessionIsValid)
            // {
            //       var news = await LoginAsync(amico.ip, isFirst: false);
            //       session = news.Session;
            //       await _repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            // }

            return "";
      }

      public async Task<string> CheckSessionAsync(Guid guid)
      {
            var amico = await _repo.GetAmicoByGuidAsync(guid);
            var session = amico.session;

            var res = await CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await LoginAsync(amico.ip, isFirst: false);
                  session = news.Session;
                  await _repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            }

            return session;
      }

      public async Task<string> CheckSessionAsync(string ip, string session)
      {

            var res = await CheckSession(ip, session);

            if (!res.SessionIsValid)
            {
                  var news = await LoginAsync(ip, isFirst: false);
                  session = news.Session;
                  await _repo.UpdateSessionByIpAsync(ip, news.Session);
            }

            return session;
      }
}