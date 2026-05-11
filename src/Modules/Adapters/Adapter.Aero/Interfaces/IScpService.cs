using System;
using Adapter.Aero.Model;

namespace AeroAdapter.Application.Interfaces;

public interface IScpService
{
      Task HandleIdReport(SCPReplyMessageDto.SCPReplyIDReportDto id);
      Task<bool> SendASCIICommandAsync(string Command);
      Task AssignIpAddressAsync(int ScpId,SCPReplyMessageDto.CC_WEB_CONFIG_NETWORKDto message);
      Task AssignPortAsync(int ScpId,SCPReplyMessageDto.CC_WEB_CONFIG_HOST_COMM_PRIMDto message);
      Task<bool> VerifySCPStructureMemoryAllocate(int ScpId,SCPReplyMessageDto.SCPReplyStrStatusDto message);
      Task<bool> UploadScpComponentAsync(int ScpId);
      Task<bool> VerifyScpComponentAsync(int ScpId);
      Task InitialScpConfiguration(short ScpId);


}
