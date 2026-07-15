using Adapter.Amico.Interfaces;

namespace Adapter.Amico.Interface;

public interface ICameraCommand : IBaseCommand
{
      Task<Stream> CaptureAsync(string Ip,string Session);
}