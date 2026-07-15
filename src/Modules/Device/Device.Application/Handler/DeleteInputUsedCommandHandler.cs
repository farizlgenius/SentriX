using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class DeleteInputUsedCommandHandler(IDeviceRepository repo) : ICommandHandler<DeleteInputUsedCommand>
{

      public async Task HandleAsync(DeleteInputUsedCommand command, CancellationToken ct)
      {

            await repo.DeleteInputAsync(command.Guid);
      }
}
