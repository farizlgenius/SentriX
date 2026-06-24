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
                  0,
                  command.ReaderNumber,
                  command.ModuleId,
                  command.LocationId,
                  true
                  );
                  
            await repo.AddReaderAsync(domain);
      }
}
