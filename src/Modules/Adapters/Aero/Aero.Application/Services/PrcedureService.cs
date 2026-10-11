using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Device;
using Core.Contract.Commands.Events;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class ProcedureService(
      IMessageBus bus,
      IProcedureRepository repo) : IProcedureAdapter
{
      public async Task AddActionAsync(string mac, string ip,short deviceId,short procId, string metadata,CancellationToken ct = default)
      {
            var meta = JsonHelper.Deserialize<ProcedureMetadata>(metadata);
            if (meta == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("ProcMetadata"));
                        
            var res = repo.ActionSpecification(
                  mac,
                  deviceId,
                  procId,
                  meta.Values
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }

}