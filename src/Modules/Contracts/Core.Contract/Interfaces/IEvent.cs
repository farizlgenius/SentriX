
using Core.Contract.DTOs.Events.AdapterEvent;
using Core.Contract.DTOs.Events.Event;
using Core.Contract.DTOs.Events.ExceptionEvent;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IEvent : IBase<EventDto, CreateEventDto, UpdateEventDto>
{

      Task<Pagination<AdapterEventDto>> GetAdapterPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task UpdateAdapterEventStatusAsync(string mac,int componentId,int tag,CommandStatus status,string reason,CancellationToken ct= default);
      Task<Pagination<ExceptionEventDto>> GetExceptionPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task InsertExceptionEventAsync(
            string path,
            string exception,
            string innerException,
            string stackTrace,
            CancellationToken ct = default);
}