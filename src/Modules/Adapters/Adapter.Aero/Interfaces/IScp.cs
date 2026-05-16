using System;
using Adapter.Aero.Model;

namespace Adapter.Aero.Interfaces;

public interface IScp
{
      Task HandleIdReport(SCPReplyMessageDto.SCPReplyIDReportDto id);
      // Task<bool> SendASCIICommandAsync(ASCIICommandDto Command);
      Task<bool> VerifySCPStructureMemoryAllocate(int ScpId, SCPReplyMessageDto.SCPReplyStrStatusDto message);
      Task<bool> UploadScpComponentAsync(int ScpId);
      Task<bool> VerifyScpComponentAsync(int ScpId);
      Task InitialScpConfigurationAsync(int ScpId);

}
