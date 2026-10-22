using Core.Application.Interfaces;
using Core.Contract.DTOs.Action;
using Core.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class ActionService(
      IActionRepository repo,
      IDeviceRepository device,
      ILocationRepository loc
      ) : BaseService<ActionDto,Domain.Entities.Actions>(
            repo
      ),IAction
{
      public async Task<Guid> CreateAsync(CreateActionDto dto, CancellationToken ct = default)
      {
            var deviceId = await device.GetIdByGuidAsync(dto.DeviceGuid,ct);
            var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid,ct);

            var d = new Domain.Entities.Actions(
                  dto.Name,
                  dto.Type,
                  dto.Metadata,
                  deviceId,
                  locationId
            );

            await repo.AddAsync(d,ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            // Check any relate first 

            if(await repo.IsAnyRelatedEntitiesAsync(guid,ct))
                  throw new FoundRelateException();

            await repo.DeleteAsync(guid,ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }


      public Task<Guid> UpdateAsync(UpdateActionDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}