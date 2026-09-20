using Core.Application.Interfaces;
using Core.Contract.Commands.Events;
using Core.Contract.DTOs.AdapterEvent;
using Core.Domain.Entities;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Events;

public sealed class AdapterEventCommandHandler(
      IEventRepository repo,
      IDeviceRepository device
      ) : ICommandHandler<AdapterEventCommand>
{
      public async Task HandleAsync(AdapterEventCommand command, CancellationToken ct)
      {
            var devices = await device.GetNameAndLocationIdByMacAsync(command.@event.Mac);
            
            var data = new AdapterEvent(
                  devices.Item1,
                  command.@event.Mac,
                  command.@event.ScpId,
                  command.@event.Command,
                  command.@event.Tag,
                  command.@event.SendAt,
                  command.@event.ReceivedAt,
                  command.@event.Body ?? string.Empty,
                  command.@event.Status,
                  command.@event.Reason,
                  string.Empty,
                  command.@event.Vendor,
                  devices.Item2
            );
            
            await repo.AddAdapterAsync(data,ct);
      }
}