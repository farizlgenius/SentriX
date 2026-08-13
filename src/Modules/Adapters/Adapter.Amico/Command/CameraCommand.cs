using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Helper;
using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Request;
using SharedKernel.Interfaces;

namespace Adapter.Amico.Command;

public sealed class CameraCommand(IHttpClient client, IAmicoSetting setting, IAmicoRepository repo) : BaseCommand(client, setting, repo), ICameraCommand
{
      public async Task<Stream> CaptureAsync(string ip, string session)
      {
            var queryParams = new Dictionary<string, string?>
            {
                  ["session"] = session,
            };

            return await Client.SendStreamAsync<CaptureRequest>(
                  HttpMethod.Post,
                  UriHelper.UriBuilder(ip, Setting.Secure),
                  Endpoint.CAPTURE,
                  new CaptureRequest(
                        "camera",
                        "rgb"
                  ),
                  queryParams: queryParams
            );
      }
}