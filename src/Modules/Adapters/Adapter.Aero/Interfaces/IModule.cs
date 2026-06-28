using Adapter.Aero.Model;

public interface IModule
{
      Task HandleFoundSioAsync(int ScpId,SCPReplyMessageDto.SCPReplySrSioDto message);
      Task<string> GetNameByMacAndComponentIdAsync(string Mac,short ComponentId,CancellationToken ct = default);
      
}