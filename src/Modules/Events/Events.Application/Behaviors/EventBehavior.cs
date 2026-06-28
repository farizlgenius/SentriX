using System;
using Device.Contract.Queries;
using Events.Application.Interfaces;
using Events.Contract.DTOs;
using Events.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Messaging;

namespace Events.Application.Behaviors;

public sealed class EventBehavior(IEventRepository repo,IMessageBus bus) : Events.Contract.Interfaces.IEvent
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
            int locationId
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
                  locationId
            );
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
