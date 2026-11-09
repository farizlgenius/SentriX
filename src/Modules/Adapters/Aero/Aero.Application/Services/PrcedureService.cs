using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Device;
using Core.Contract.Commands.Events;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class ProcedureService(
      IMessageBus bus,
      IProcedureRepository repo) : IProcedureAdapter
{
      public async Task AddProcedureAsync(string mac, string ip,short deviceId,short procId, Dictionary<ActionType,string> metadata,CancellationToken ct = default)
      {
                        
            var res = repo.ActionSpecification(
                  mac,
                  deviceId,
                  procId,
                  metadata
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }

      public async Task DeleteProcedureAsync(string mac, string ip, short deviceId, short procId, CancellationToken ct = default)
      {
            var res = repo.ActionSpecification(
                  mac,
                  deviceId,
                  procId,
                  []
            );
      }
}