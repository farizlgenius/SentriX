using Core.Application.Interfaces;
using Core.Contract.DTOs.Trigger;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class TriggerVariableService(
      ITriggerVariableRepository repo,
      ITriggerRepository trigger,
      IDeviceRepository device
) : BaseService<TriggerVariableDto,TriggerVariable>(repo),ITriggerVariable
{
      public async Task<Guid> CreateAsync(CreateTriggerVariableDto dto, CancellationToken ct = default)
      {
            var triggerId = await trigger.GetIdByGuidAsync(dto.TriggerGuid,ct);
            var deviceId = await device.GetIdByGuidAsync(dto.DeviceGuid,ct);
            var externalId = await Repo.GetFreeExternalIdAsync(127,[],ct);

            var d = new Domain.Entities.TriggerVariable(
                  triggerId,
                  deviceId,
                  externalId
            );

            await Repo.AddAsync(d,ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            if(await Repo.IsAnyRelatedEntitiesAsync(guid,ct))
                  throw new FoundRelateException();

            await Repo.DeleteAsync(guid,ct);

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


      public async Task<Guid> UpdateAsync(UpdateTriggerVariableDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}