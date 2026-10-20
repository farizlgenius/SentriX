using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Output;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Services;

public sealed class OutputService(
      IOutputRepository repo,
      IDeviceRepository device,
      IDeviceModuleRepository module,
      IAdapterFactory adapter,
      IComponentMappingRepository com,
      ILocationRepository loc) : IOutput
{
      public async Task<Guid> CreateAsync(CreateOutputDto dto, CancellationToken ct = default)
      {
            var dev = await device.GetAsync(dto.DeviceGuid,ct);
            var deviceModuleId = await module.GetIdByGuidAsync(dto.DeviceModuleGuid,ct);
            var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid,ct);

            var d = new Output(
                  dto.Name,
                  dto.SlotNo,
                  dto.Metadata,
                  dto.Vendor,
                  deviceModuleId,
                  locationId
            );

            var deviceExternalId = await com.GetExternalIdByGuidAndEntityAsync(dto.DeviceGuid,SharedKernel.Enums.EntityType.DEVICE,ct);
            var moduleExternalId = await com.GetExternalIdByGuidAndEntityAsync(dto.DeviceModuleGuid,SharedKernel.Enums.EntityType.DEVICE_MODULE,ct);
            var controlExternalId = await com.GetFreeIdByMacAndEntityAndVendorAsync(dev.Mac,SharedKernel.Enums.EntityType.OUTPUT,SharedKernel.Enums.Vendor.aero,100,[],ct);

            // Send command to create output 
            await adapter.GetAdapter(dto.Vendor).Output.CreateControlPointAsync(
                  dev.Mac,
                  dev.Ip,
                  (short)deviceExternalId,
                  (short)moduleExternalId,
                  (short)dto.SlotNo,
                  (short)controlExternalId,
                  dto.Metadata,
                  ct
                  );

            await repo.AddAsync(d,ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var output = await repo.GetAsync(guid,ct);
            var dev = await device.GetAsync(output.DeviceGuid,ct);

            var deviceExternalId = await com.GetExternalIdByGuidAndEntityAsync(output.DeviceGuid,SharedKernel.Enums.EntityType.DEVICE,ct);
            var controlExternalId = await com.GetExternalIdByGuidAndEntityAsync(guid,SharedKernel.Enums.EntityType.DEVICE_MODULE,ct);

            await adapter.GetAdapter(output.Vendor).Output.DeleteControlPointAsync(
                  dev.Mac,
                  dev.Ip,
                  (short)deviceExternalId,
                  (short)output.SlotNo,
                  (short)controlExternalId,
                  ct
            );

            await repo.DeleteAsync(guid,ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            foreach (var guid in guids)
            {
                  var output = await repo.GetAsync(guid,ct);
                  var dev = await device.GetAsync(output.DeviceGuid,ct);

                  var deviceExternalId = await com.GetExternalIdByGuidAndEntityAsync(output.DeviceGuid, SharedKernel.Enums.EntityType.DEVICE, ct);
                  var controlExternalId = await com.GetExternalIdByGuidAndEntityAsync(guid, SharedKernel.Enums.EntityType.DEVICE_MODULE, ct);

                  await adapter.GetAdapter(output.Vendor).Output.DeleteControlPointAsync(
                        dev.Mac,
                        dev.Ip,
                        (short)deviceExternalId,
                        (short)output.SlotNo,
                        (short)controlExternalId,
                        ct
                  );
            }

            await repo.DeleteRangeAsync(guids,ct);

            return guids;
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            var output = await repo.GetAsync(guid,ct);
            var dev = await device.GetAsync(output.DeviceGuid,ct);

            var deviceExternalId = await com.GetExternalIdByGuidAndEntityAsync(output.DeviceGuid,SharedKernel.Enums.EntityType.DEVICE,ct);
            var controlExternalId = await com.GetExternalIdByGuidAndEntityAsync(guid,SharedKernel.Enums.EntityType.DEVICE_MODULE,ct);

            await adapter.GetAdapter(output.Vendor).Output.DeleteControlPointAsync(
                  dev.Mac,
                  dev.Ip,
                  (short)deviceExternalId,
                  (short)output.SlotNo,
                  (short)controlExternalId,
                  ct
            );

            await repo.DisableAsync(guid,ct);

            return true;
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            var output = await repo.GetAsync(guid,ct);
            var dev = await device.GetAsync(output.DeviceGuid,ct);


            var deviceExternalId = await com.GetExternalIdByGuidAndEntityAsync(output.DeviceGuid,SharedKernel.Enums.EntityType.DEVICE,ct);
            var moduleExternalId = await com.GetExternalIdByGuidAndEntityAsync(output.DeviceModuleGuid,SharedKernel.Enums.EntityType.DEVICE_MODULE,ct);
            var controlExternalId = await com.GetExternalIdByGuidAndEntityAsync(guid,SharedKernel.Enums.EntityType.DEVICE_MODULE,ct);

            // Send command to create output 
            await adapter.GetAdapter(output.Vendor).Output.CreateControlPointAsync(
                  dev.Mac,
                  dev.Ip,
                  (short)deviceExternalId,
                  (short)moduleExternalId,
                  (short)output.SlotNo,
                  (short)controlExternalId,
                  output.Metadata,
                  ct
                  );

            await repo.EnableAsync(guid,ct);

            return true;
      }

      public async Task<OutputDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<OutputDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Guid> UpdateAsync(UpdateOutputDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task UploadAsync(CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}