using Core.Application.Interfaces;
using Core.Contract.DTOs.Alert;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class AlertService(
      IAlertRepository repo,
      IEventRepository @event,
      IDoorRepository door,
      ILocationRepository loc
      ) : IAlert
{
      public async Task<Guid> CreateAsync(CreateAlertDto dto, CancellationToken ct = default)
      {
            var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid,ct);
            var levelId = await repo.GetLevelIdByGuidAsync(dto.AlertLevelGuid,ct);
            var eventId = await @event.GetCoreEventIdByGuidAsync(dto.CoreEventGuid);
            var entity = await @event.GetCoreEntityByGuidAsync(dto.CoreEntityGuid);
            IEnumerable<int> components = new List<int>();

            switch (entity)
            {
                  case EntityType.DOOR:
                  components = await door.GetIdsByGuidsAsync(dto.Components,ct);
                  break;
                  default:
                  break;
            }

            var d = new Domain.Entities.Alert(
                  dto.Name,
                  dto.Advice,
                  locationId,
                  levelId,
                  eventId,
                  entity,
                  components.ToList()
            );

            await repo.AddAsync(d,ct);

            return d.Guid;
      }

      public async Task<Guid> CreateLevelAsync(CreateAlertLevelDto dto, CancellationToken ct = default)
      {
            var d = new AlertLevel(
                  dto.Name,
                  dto.ColorCode
            );

            await repo.AddLevelAsync(d,ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            
            await repo.DeleteAsync(guid,ct);

            return true;
      }


      public async Task<bool> DeleteLevelByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            // check relation
            if(await repo.IsAnyLevelRelatedEntitiesAsync(guid,ct))
                  throw new FoundRelateException();

            await repo.DeleteLevelAsync(guid,ct);

            return true;
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

      public async Task<AlertDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<AlertDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Pagination<AlertEventDto>> GetEventPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetEventPaginationAsync(param,ct);
      }

      public async Task<IEnumerable<OptionDto>> GetLevelOptionAsync(CancellationToken ct = default)
      {
            return await repo.GetLevelOptionAsync();
      }

      public async Task<Pagination<AlertLevelDto>> GetLevelPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetLevelPaginationAsync(param,ct);
      }

      public async Task<Pagination<AlertDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetPaginationAsync(param,ct);
      }

      public async Task<Guid> UpdateAsync(UpdateAlertDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Guid> UpdateLevelAsync(UpdateAlertLevelDto dto, CancellationToken ct = default)
      {
            // Check is any Company with guid
            if (!await repo.IsAnyLevelByGuidAsync(dto.Guid, ct))
                  throw new NotFoundException(EntityType.COMPANY.ToString(), dto.Guid.ToString());

            var d = new Core.Domain.Entities.AlertLevel(
                  dto.Guid,
                  dto.Name,
                  dto.ColorCode
            );

            await repo.UpdateLevelAsync(d,ct);

            return d.Guid;
      }
}