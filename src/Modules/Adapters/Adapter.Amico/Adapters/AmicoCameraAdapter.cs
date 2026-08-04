using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Amico.Adapters;

public sealed class AmicoCameraAdapter(
      ICameraCommand command,
      IAmicoRepository repo
      ) : IAmicoCameraAdapter
{
      public async Task<Stream> CaptureAsync(string Ip,string Mac)
      {
            var session = await command.CheckSessionAsync(Mac);
            return await command.CaptureAsync(Ip,session);
      }
}