using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Interface;

public interface IDeviceCommand 
{
      Task<DeviceInfoResponse> DeviceInfoAsync(string ip,string session);
      Task<LoginResponse> LoginAsync(string ip,string login,string password); 
      Task<bool> LogoutAsync(string ip,string session);
      Task<CheckSessionResponse> CheckSession(string ip,string session);
}