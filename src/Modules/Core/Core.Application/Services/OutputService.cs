using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Output;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class OutputService(
      IOutputRepository repo,
      IDeviceModuleRepository module,
      IAdapterFactory adapter) : BaseService<OutputDto, Domain.Entities.Output>(
            repo
            ), IOutput
{
      public async Task<Guid> CreateAsync(CreateOutputDto dto, CancellationToken ct = default)
      {

            var dev = await module.GetCommandDetailAsync(dto.DeviceModuleGuid, ct);

            if (await repo.IsAnyByNameAsync(dto.Name, ct))
                  throw new DuplicateException(EntityType.OUTPUT.ToString(), nameof(dto.Name));

            var controlExternalId = await repo.GetFreeExeternalIdAsync(100, [], ct);

            var d = new Output(
                  dto.Name,
                  dto.SlotNo,
                  dto.Metadata,
                  dto.Vendor,
                  dev.ModuleId,
                  dev.LocationId ?? 1,
                  controlExternalId
            );

            
            // Send command to create output 
            await adapter.GetAdapter(dto.Vendor).Output.CreateControlPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ModuleExternalId,
                  (short)dto.SlotNo,
                  (short)controlExternalId,
                  dto.Metadata,
                  ct
                  );

            await repo.AddAsync(d, ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await repo.GetCommandDetailAsync(guid, ct);
            var output = await repo.GetAsync(guid, ct);

            await adapter.GetAdapter(dev.Vendor).Output.DeleteControlPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  (short)output.SlotNo,
                  dev.ComponentExternalId ?? -1,
                  ct
            );

            await repo.DeleteAsync(guid, ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            foreach (var guid in guids)
            {
                  var dev = await repo.GetCommandDetailAsync(guid, ct);
                  var output = await repo.GetAsync(guid, ct);

                  await adapter.GetAdapter(dev.Vendor).Output.DeleteControlPointAsync(
                        dev.DeviceMac,
                        dev.DeviceIp,
                        dev.DeviceExternalId,
                        (short)output.SlotNo,
                        dev.ComponentExternalId ?? -1,
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
            var output = await repo.GetAsync(guid, ct);

            await adapter.GetAdapter(dev.Vendor).Output.DeleteControlPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  (short)output.SlotNo,
                  dev.ComponentExternalId ?? -1,
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
            var output = await repo.GetAsync(guid, ct);


            await adapter.GetAdapter(output.Vendor).Output.CreateControlPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ModuleExternalId,
                  (short)output.SlotNo,
                  dev.ComponentExternalId ?? -1,
                  output.Metadata,
                  ct
                  );

            await repo.EnableAsync(guid, ct);

            return true;
      }

      public async Task<bool> OutputCommandAsync(OutputCommandDto command, CancellationToken ct = default)
      {
            var output = await Repo.GetAsync(command.Guid,ct);

            var dev = await Repo.GetCommandDetailAsync(command.Guid,ct);
            
            await adapter.GetAdapter(output.Vendor).Output.ControlPointCommandAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  command.Command,
                  command.OnTime,
                  command.OffTime,
                  command.Repeat
            );

            return true;
      }

      public async Task<Guid> UpdateAsync(UpdateOutputDto dto, CancellationToken ct = default)
      {

            var dev = await repo.GetCommandDetailAsync(dto.Guid, ct);

            var d = new Output(
                  dto.Guid,
                  dto.Name,
                  dto.SlotNo,
                  dto.Metadata,
                  dto.Vendor,
                  dev.ModuleId,
                  dev.LocationId ?? 1
            );

            // Send command to create output 
            await adapter.GetAdapter(dto.Vendor).Output.CreateControlPointAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ModuleExternalId,
                  (short)dto.SlotNo,
                  dev.ComponentExternalId ?? -1,
                  dto.Metadata,
                  ct
                  );

            await repo.UpdateAsync(d, ct);

            return d.Guid;
      }

      public async Task UploadAsync(CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}