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
            var domain = new Reader(
                  0,
                  command.ReaderNumber,
                  command.ModuleId,
                  0,
                  true
            );
            await repo.DeleteReaderAsync(domain);
      }
}
