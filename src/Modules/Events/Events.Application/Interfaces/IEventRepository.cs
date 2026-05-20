using System;
using Events.Contract.DTOs;
using SharedKernel.Domain;
using SharedKernel.Model;

namespace Events.Application.Interfaces;

public interface IEventRepository
{
      Task<Pagination<EventDto>> GetPaginationByLocationIdAsync(PaginationParams param);
      Task AddAsync(
            DateTime timeStamp,
            string actor,
            string module,
            string type,
            string image,
            string mac,
            string name,
            string remarks,
            int locationId
            );

      Task AddCommandEvent(CommandResponse response);
}
