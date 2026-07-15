using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class AddReaderUsedCommandHandler(IDeviceRepository repo) : ICommandHandler<AddReaderUsedCommand>
{

      public async Task HandleAsync(AddReaderUsedCommand command, CancellationToken ct)
      {
            var domain = new Reader(
                  Guid.NewGuid(),
                  command.ReaderNumber,
                  command.ModuleGuid,
                  command.LocationId
                  );
                  
            await repo.AddReaderAsync(domain);
      }
}
