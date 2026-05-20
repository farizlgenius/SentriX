using System;
using Events.Contract.DTOs;
using SharedKernel.Domain;
using SharedKernel.Model;

namespace Events.Application.Interfaces;

public interface IEventRepository
{
      Task<Pagination<EventDto>> GetPaginationByLocationIdAsync(PaginationParams param,CancellationToken ct = default);
      Task AddAsync(
            DateTime timeStamp,
            string actor,
            string module,
            string type,
            string image,
            string mac,
            string name,
            string remarks,
            int locationId,
            CancellationToken ct = default
            );

      Task AddCommandEvent(CommandResponse response,CancellationToken ct = default);
      Task UpdateCommandEvent(
            string Mac, int Tag, short CommandStatus, string Reason,CancellationToken ct = default
      );
}
