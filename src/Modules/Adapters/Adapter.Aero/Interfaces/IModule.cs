using Adapter.Aero.Model;

public interface IModule
{
      Task HandleFoundSioAsync(int ScpId,SCPReplyMessageDto.SCPReplySrSioDto message);
      
}