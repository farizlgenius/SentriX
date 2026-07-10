using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Interfaces;

public interface IBaseCommand
{ 
       Task<LoginResponse> LoginAsync(string ip); 
      Task<bool> LogoutAsync(string ip,string session);
      Task<CheckSessionResponse> CheckSession(string ip,string session);
}