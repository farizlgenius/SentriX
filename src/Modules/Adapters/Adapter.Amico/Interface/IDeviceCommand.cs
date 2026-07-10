using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Interface;

public interface IDeviceCommand : IBaseCommand
{
      Task<DeviceInfoResponse> DeviceInfoAsync(string ip,string session);
      Task ChangeLogin(string ip,string login, string password,string session);
}