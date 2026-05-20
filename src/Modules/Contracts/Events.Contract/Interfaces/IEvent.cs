using System;
using Events.Contract.DTOs;
using SharedKernel.Domain;

namespace Events.Contract.Interfaces;

public interface IEvent
{
      Task<Pagination<EventDto>> GetPaginationByLocationIdAsync(PaginationParams param);
      Task AddEventAsync( 
            DateTime TimeStamp,
            int ComponentId,
            string Actor,
            string Module,
            string Type,
            string Remarks);

      Task UpdateCommandEvent(
            int ComponentId,
            int Tag,
            short CommandStatus,
            string Reason
            );

      
}
