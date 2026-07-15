namespace Adapter.Amico.Interface;

public interface IAmicoCameraAdapter 
{
      Task<Stream> CaptureAsync(string ip);
}