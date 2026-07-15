using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Amico.Adapters;

public sealed class AmicoCameraAdapter(
      ICameraCommand command,
      IAmicoRepository repo
      ) : IAmicoCameraAdapter
{
      public async Task<Stream> CaptureAsync(string Mac)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            var res = await command.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await command.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac,news.Session);
            }
            return await command.CaptureAsync(amico.ip,session);
      }
}