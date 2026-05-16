using Adapter.Aero.Interfaces;
using Adapter.Aero.Model;
using Device.Contract.Command;
using Microsoft.Extensions.Logging;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class SioService(ISioRepository repo,IScpRepository scp,IMessageBus bus) : ISio
{

      public async Task HandleFoundSioAsync(int ScpId, SCPReplyMessageDto.SCPReplySrSioDto message)
      {
            // Check that any sio in database 
            var flag = await repo.IsAnySioByScpIdAndSioIdAsync(ScpId,message.number);
            var Mac = await scp.MacByScpIdAsync(ScpId);
            var MpduleId = await repo.GetModuleIdByScpIdAndSioIdAsync(ScpId,message.number);

            if (flag)
            {
                  await bus.SendAsync(new ModuleUpdateCommand(
                        Mac,
                        MpduleId,
                        message.serial_number.ToString(),
                        message.revision.ToString(),
                        message.msp1_dnum));
            }

      }
}