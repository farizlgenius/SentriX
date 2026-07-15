using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class DeleteReaderUsedCommandHandler(IDeviceRepository repo) : ICommandHandler<DeleteReaderUsedCommand>
{

      public async Task HandleAsync(DeleteReaderUsedCommand command, CancellationToken ct)
      {
            await repo.DeleteReaderAsync(command.Guid);
      }
}
