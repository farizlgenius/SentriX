using Core.Contract.DTOs.AdapterEvent;
using Core.Contract.DTOs.Event;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IEvent : IBase<EventDto, CreateEventDto, UpdateEventDto>
{
      Task<Pagination<AdapterEventDto>> GetAdapterPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task UpdateAdapterEventStatusAsync(int componentId,int tag,CommandStatus status,string reason,CancellationToken ct= default);
}