using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Interfaces;

public interface IBaseCommand
{ 
      //  Task<LoginResponse> LoginAsync(string ip,bool? isFirst = false); 
      // Task<bool> LogoutAsync(string ip,string session);
      // Task<CheckSessionResponse> CheckSession(string ip,string session);
      Task<string> CheckSessionAsync(string mac);
      Task<string> CheckSessionAsync(Guid guid);
      Task<string> CheckSessionAsync(string ip, string session);
      Task<LoginResponse> LoginAsync(string ip,bool? isFirst);
 
}