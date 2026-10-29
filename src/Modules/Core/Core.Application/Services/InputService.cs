using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Input;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class InputService(
      IInputRepository repo,
      IAdapterFactory adapter,
      IDeviceModuleRepository module
) : BaseService<InputDto, Domain.Entities.Input>(
      repo
), IInput
{
      public async Task<Guid> CreateAsync(CreateInputDto dto, CancellationToken ct = default)
      {
            var dev = await module.GetCommandDetailAsync(dto.DeviceModuleGuid, ct);

            if (await repo.IsAnyByNameAsync(dto.Name, ct))
                  throw new DuplicateException(EntityType.INPUT.ToString(), nameof(dto.Name));

            var inputExternalId = await repo.GetFreeExeternalIdByMacAsync(dev.DeviceMac,100, [], ct);

            var d = new Input(
                  dto.Name,
                  dto.SlotNo,
                  dto.Metadata,
                  dto.Vendor,
                  dev.ModuleId,
                  dev.LocationId ?? 1,
                  inputExternalId
            );

            await adapter.GetAdapter(dev.Vendor).Input.MonitorPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ModuleExternalId,
                  inputExternalId,
                  d.SlotNo,
                  d.Metadata,
                  ct
            );

            await Repo.AddAsync(d,ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            if(await Repo.IsAnyRelatedEntitiesAsync(guid,ct))
                  throw new FoundRelateException();

            var dev = await Repo.GetCommandDetailAsync(guid,ct);

            var input = await Repo.GetAsync(guid,ct);

            await adapter.GetAdapter(dev.Vendor).Input.DeleteMonitorPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  input.SlotNo,
                  input.Metadata,
                  ct
            );

            await Repo.DeleteAsync(guid,ct);

            return true;
            
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            foreach (var guid in guids)
            {
                  var dev = await repo.GetCommandDetailAsync(guid, ct);
                  var input = await repo.GetAsync(guid, ct);

                  await adapter.GetAdapter(dev.Vendor).Input.DeleteMonitorPointAsync(
                        dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  input.SlotNo,
                  input.Metadata,
                  ct
                  );
            }

            await repo.DeleteRangeAsync(guids, ct);

            return guids;
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            if (await repo.IsDisabledAsync(guid, ct))
                  return true;

            var dev = await repo.GetCommandDetailAsync(guid, ct);

            var input = await Repo.GetAsync(guid,ct);

            await adapter.GetAdapter(dev.Vendor).Input.DeleteMonitorPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  input.SlotNo,
                  input.Metadata,
                  ct
            );

            await repo.DisableAsync(guid, ct);

            return true;
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            if (await repo.IsDisabledAsync(guid, ct))
                  return true;


            var dev = await repo.GetCommandDetailAsync(guid, ct);
             var input = await Repo.GetAsync(guid,ct);

            await adapter.GetAdapter(dev.Vendor).Input.DeleteMonitorPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  input.SlotNo,
                  input.Metadata,
                  ct
            );

            await repo.EnableAsync(guid, ct);

            return true;
      }

      public async Task<bool> GetStatusAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid,ct);

            await adapter.GetAdapter(dev.Vendor).Input.MonitorPointStatusAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  1,
                  ct
                  );

            return true;
      }

      public async Task<bool> MaskAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid,ct);

            await adapter.GetAdapter(dev.Vendor).Input.MaskMonitorPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );

            return true;
      }

      public async Task<bool> UnmaskAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid,ct);

            await adapter.GetAdapter(dev.Vendor).Input.MaskMonitorPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );

            return true;
      }

      public async Task<Guid> UpdateAsync(UpdateInputDto dto, CancellationToken ct = default)
      {
            var dev = await repo.GetCommandDetailAsync(dto.Guid, ct);

            var d = new Input(
                  dto.Name,
                  dto.SlotNo,
                  dto.Metadata,
                  dto.Vendor,
                  dev.ModuleId,
                  dev.LocationId ?? 1,
                  dev.ComponentExternalId ?? -1
            );

            await adapter.GetAdapter(dev.Vendor).Input.MonitorPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ModuleExternalId,
                  dev.ComponentExternalId ?? -1,
                  d.SlotNo,
                  d.Metadata,
                  ct
            );

            await Repo.UpdateAsync(d,ct);

            return d.Guid;
      }
}