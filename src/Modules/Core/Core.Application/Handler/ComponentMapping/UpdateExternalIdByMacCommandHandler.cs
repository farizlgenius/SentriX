using Core.Contract.Commands.ComponentMapping;
using Core.Contract.Interfaces;
using SharedKernel.Messaging;

namespace Core.Application.Handler.ComponentMapping;

public sealed class UpdateExternalIdByMacCommandHandler(IComponentMapping component) : ICommandHandler<UpdateExternalIdByMacCommand>
{
      public async Task HandleAsync(UpdateExternalIdByMacCommand command, CancellationToken ct)
      {
            await component.UpdateExternalIdByMacAsync(command.mac,command.externalId,ct);
      }
}