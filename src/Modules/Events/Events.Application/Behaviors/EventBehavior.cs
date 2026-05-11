using System;
using Events.Application.Interfaces;
using Events.Contract.DTOs;
using Events.Contract.Interfaces;
using SharedKernel.Domain;

namespace Events.Application.Behaviors;

public sealed class EventBehavior(IEventRepository repo) : IEvent
{
      public async Task AddEventAsync(
             DateTime timeStamp,
            string actor,
            string module,
            string type,
            string image,
            string mac,
            string name,
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
                  remarks,
                  locationId
            );
      }

      public async Task<Pagination<EventDto>> GetPaginationByLocationIdAsync(PaginationParams param, string tenant_id)
      {
            return await repo.GetPaginationByLocationIdAsync(param, tenant_id);
      }
}
