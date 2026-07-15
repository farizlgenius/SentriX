using System;
using Events.Contract.DTOs;
using SharedKernel.Domain;

namespace Events.Contract.Interfaces;

public interface IEvent
{
      Task<Pagination<EventDto>> GetPaginationByLocationIdAsync(PaginationParams param);
      Task<Pagination<CommandEventDto>> GetCommandPaginationAsync(PaginationParams param);
      Task AddEventAsync( 
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
            string capture=""
           );

      Task UpdateCommandEvent(
            int ComponentId,
            int Tag,
            short CommandStatus,
            string Reason
            );

      Task<Stream> GetCaptureByTimeAsync(string time,CancellationToken ct = default);

      
}
