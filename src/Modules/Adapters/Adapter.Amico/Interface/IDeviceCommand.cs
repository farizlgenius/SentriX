using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Interface;

public interface IDeviceCommand : IBaseCommand
{
      Task<bool> VerifyDeviceComponentAsync(string ip,string session,int location_id); 
      Task<DeviceInfoResponse> DeviceInfoAsync(string ip,string session);
      Task ChangeLogin(string ip,string session);
}