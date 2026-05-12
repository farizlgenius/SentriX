using System;
using Events.Contract.DTOs;
using SharedKernel.Domain;

namespace Events.Contract.Interfaces;

public interface IEvent
{
      Task<Pagination<EventDto>> GetPaginationByLocationIdAsync(PaginationParams param);
      Task AddEventAsync( DateTime timeStamp,
            string actor,
            string module,
            string type,
            string image,
            string mac,
            string name,
            string remarks,
            int locationId);
}
