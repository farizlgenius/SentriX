using Adapter.Aero.Model;

public interface ISio
{
      Task HandleFoundSioAsync(int ScpId,SCPReplyMessageDto.SCPReplySrSioDto message);
      
}