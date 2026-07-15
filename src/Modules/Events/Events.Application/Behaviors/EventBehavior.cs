using System;
using Device.Contract.Queries;
using Events.Application.Interfaces;
using Events.Contract.DTOs;
using Events.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using Storage.Contract.Interfaces;

namespace Events.Application.Behaviors;

public sealed class EventBehavior(IEventRepository repo,IMessageBus bus,IStorage file) : Events.Contract.Interfaces.IEvent
{
      public async Task AddEventAsync(
             DateTime timeStamp,
            string actor,
            string module,
            string type,
            string image,
            string mac,
            string name,
            string code,
            string remarks,
            int locationId,
            string capture
      )
      {
            await repo.AddAsync(
                  timeStamp,
                  actor,
                  module,
                  type,
                  image,
                  mac,
                  name,
                  code,
                  remarks,
                  locationId,
                  capture
            );
      }

      public async Task<Stream> GetCaptureByTimeAsync(string time, CancellationToken ct = default)
      {
             if (string.IsNullOrEmpty(time))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(time)));

            return await file.ReadCaptureAsync(time);
      }

      public async Task<Pagination<CommandEventDto>> GetCommandPaginationAsync(PaginationParams param)
      {
            return await repo.GetCommandPaginationAsync(param);
      }

      public async Task<Pagination<EventDto>> GetPaginationByLocationIdAsync(PaginationParams param)
      {
            return await repo.GetPaginationByLocationIdAsync(param);
      }

      public async Task UpdateCommandEvent(int ComponentId, int Tag, short CommandStatus, string Reason)
      {
            var Mac = await bus.QueryAsync(new MacByComponentIdQuery(ComponentId));
            await repo.UpdateCommandEvent(Mac,Tag,CommandStatus,Reason);
      }
}
