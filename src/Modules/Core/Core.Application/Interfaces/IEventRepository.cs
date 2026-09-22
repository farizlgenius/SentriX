
using Core.Contract.DTOs.Events.AdapterEvent;
using Core.Contract.DTOs.Events.Event;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IEventRepository : IBaseRepository<EventDto, Event>
{
      Task<Pagination<AdapterEventDto>> GetAdapterPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task AddAdapterAsync(AdapterEvent @event,CancellationToken ct = default);
      Task UpdateAdapterEventStatusAsync(int componentId, int tag, CommandStatus status, string reason, CancellationToken ct = default);
      Task AddExceptionAsync(
            string entity,
            string exception,
            string innerException,
            string stackTrace,
            CancellationToken ct = default
      );
}