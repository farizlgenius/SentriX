
using Core.Contract.DTOs.Events.AdapterEvent;
using Core.Contract.DTOs.Events.Audit;
using Core.Contract.DTOs.Events.Event;
using Core.Contract.DTOs.Events.ExceptionEvent;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IEventRepository : IRepository<EventDto, Event>
{
      Task<Pagination<AdapterEventDto>> GetAdapterPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<Pagination<ExceptionEventDto>> GetExceptionPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task<Pagination<AuditTrailDto>> GetAuditPaginationAsync(PaginationParams param,CancellationToken ct = default);
      Task AddAdapterAsync(AdapterEvent @event,CancellationToken ct = default);
      Task UpdateAdapterEventStatusAsync(string mac,int componentId, int tag, CommandStatus status, string reason, CancellationToken ct = default);
      Task AddExceptionAsync(
            string method,
            string entity,
            string exception,
            string innerException,
            string stackTrace,
            CancellationToken ct = default
      );

      Task AddAuditAsync(AuditTrail audit,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetCoreEventAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetCoreEntityAsync(CancellationToken ct = default);
      Task<int> GetCoreEventIdByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<int> GetCoreEntityIdByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<EntityType> GetCoreEntityByGuidAsync(Guid guid,CancellationToken ct = default);
}