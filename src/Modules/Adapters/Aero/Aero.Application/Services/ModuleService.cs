using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Core.Contract.Commands.Events;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class ModuleService(
      IMessageBus bus,
      IModuleRepository repo) : IModuleAdapter
{
      public async Task GetStatusAsync(
            string mac,
            string ip,
            short deviceId,
            short moduleId,
            CancellationToken ct = default)
      {
            var res = repo.EnCcSioSrq(
                  mac,
                  deviceId,
                  moduleId
                  );

            await bus.SendAsync(new AdapterEventCommand(res));

      }
}