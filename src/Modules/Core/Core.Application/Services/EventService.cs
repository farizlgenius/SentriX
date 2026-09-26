using Core.Application.Interfaces;
using Core.Contract.DTOs.Events.AdapterEvent;
using Core.Contract.DTOs.Events.Audit;
using Core.Contract.DTOs.Events.Event;
using Core.Contract.DTOs.Events.ExceptionEvent;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class EventService(
  IEventRepository repo,
  ILocationRepository loc,
  ILogger<EventService> logger
  ) : Core.Contract.Interfaces.IEvent
{
  public async Task<Guid> CreateAsync(CreateEventDto dto, CancellationToken ct = default)
  {
    var locationId = dto.LocationGuid == Guid.Empty ? 0 : await loc.GetIdByGuidAsync(dto.LocationGuid,ct);

    var d = new Event(
      dto.Timestamp,
      dto.Actor,
      dto.Module,
      dto.EventType,
      dto.ImageName,
      dto.Mac,
      dto.ComponentName,
      dto.EventCode,
      dto.Remarks,
      dto.CaptureImageName,
      locationId
    );

    await repo.AddAsync(d,ct);

    return d.Guid;
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

      public async Task<Pagination<AuditTrailDto>> GeAuditPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetAuditPaginationAsync(param,ct);
      }

      public async Task<Pagination<AdapterEventDto>> GetAdapterPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetAdapterPaginationAsync(param, ct);
  }

  public async Task<EventDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<EventDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Pagination<ExceptionEventDto>> GetExceptionPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetExceptionPaginationAsync(param,ct);
  }

  public async Task<Pagination<EventDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    return await repo.GetPaginationAsync(param, ct);
  }

  public async Task InsertAuditAsync(
    string entity,
    AuditAction action,
    string username,
    string ip,
    Guid? objectGuid,
    string? name,
    string? detail,
    int locationId,
    CancellationToken ct = default)
  {

    var d = new AuditTrail(
      entity,
      action,
      username,
      ip,
      objectGuid,
      name,
      detail,
      locationId
    );
    await repo.AddAuditAsync(d, ct);
  }

  public async Task InsertExceptionEventAsync(string method,string path, string exception, string innerException, string stackTrace, CancellationToken ct = default)
  {
    await repo.AddExceptionAsync(method,path,exception,innerException,stackTrace,ct);
  }

  public async Task UpdateAdapterEventStatusAsync(string mac,int componentId, int tag, CommandStatus status, string reason, CancellationToken ct = default)
  {
    await repo.UpdateAdapterEventStatusAsync(mac,componentId, tag, status, reason, ct);
  }

  public async Task<Guid> UpdateAsync(UpdateEventDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }


}