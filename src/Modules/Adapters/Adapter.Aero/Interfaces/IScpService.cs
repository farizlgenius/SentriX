using System;
using Adapter.Aero.Model;

namespace Adapter.Aero.Interfaces;

public interface IScpService
{
      Task HandleIdReport(SCPReplyMessageDto.SCPReplyIDReportDto id);
      // Task<bool> SendASCIICommandAsync(ASCIICommandDto Command);
      Task<bool> VerifySCPStructureMemoryAllocate(int ScpId, SCPReplyMessageDto.SCPReplyStrStatusDto message);
      Task VerifyScpComponentAsync(int ScpId);
      Task InitialScpConfigurationAsync(int ScpId);

}
