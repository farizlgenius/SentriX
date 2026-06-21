using System;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Domain.Entities;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class AddReaderUsedCommandHandler(IDeviceRepository repo) : ICommandHandlerWithResult<AddReaderUsedCommand,bool>
{

      public async Task<bool> HandleAsync(AddReaderUsedCommand command, CancellationToken ct)
      {
            var domain = new Reader(
                  0,
                  command.ReaderNumber,
                  command.ModuleId,
                  command.LocationId,
                  true
                  );
                  
            return await repo.AddReaderAsync(domain);
      }
}
