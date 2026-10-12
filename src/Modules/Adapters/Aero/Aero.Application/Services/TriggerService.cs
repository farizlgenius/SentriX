

using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Core.Contract.Commands.Events;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class TriggerService(
      IMessageBus bus,
      ITriggerRepository repo
      ) : ITriggerAdapter
{
      public async Task AddTriggerAsync(
      string mac,
      string ip,
      short deviceId,
      short trigId,
      string metadata,
      CancellationToken ct = default
      )
      {
            // var meta = JsonHelper.Deserialize<ProcedureMetadata>(metadata);
            // if (meta == null)
            //       throw new Exception(MessageHelper.Common.DeserializeFailed("ProcMetadata"));

            // var res = repo.TriggerSpecification(
            //       mac,
            //       deviceId,
            //       trigId,

            // );

            // await bus.SendAsync(new AdapterEventCommand(res), ct);
      }

}