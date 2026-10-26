using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Area;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class AreaService(
      IAreaRepository repo,
      IDeviceRepository device,
      IDoorRepository door,
      IAdapterFactory adapter
      ) : BaseService<AreaDto,Domain.Entities.Area>(
      repo
),IArea
{
      public async Task<Guid> CreateAsync(CreateAreaDto dto, CancellationToken ct = default)
      {
            foreach(var dor in dto.AreaInDoors)
            {
                  if(dto.AreaOutDoors.Contains(dor))
                        throw new BadRequestException("Duplicate in / out area doors.");
            }

            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid,ct);

            var areaId = await repo.GetFreeExeternalIdAsync(100,[],ct);

            var doorIns = await door.GetIdsByGuidsAsync(dto.AreaInDoors,ct);
            var doorOuts = await door.GetIdsByGuidsAsync(dto.AreaOutDoors,ct);

            var d = new Area(
                  dto.Name,
                  dto.MultiMode,
                  dto.Mode,
                  dto.OccMax,
                  dto.OccDown,
                  dto.OccUp,
                  dto.Metadata,
                  [.. doorIns],
                  [.. doorOuts],
                  dev.DeviceId,
                  areaId
            );

            // Send Area

            await adapter.GetAdapter(dev.Vendor).Area.AddAreaAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  areaId,
                  dto.MultiMode,
                  dto.Mode,
                  dto.OccMax,
                  dto.OccDown,
                  dto.OccUp,
                  dto.Metadata,
                  ct
            );

            // Send Door In Command
            foreach(var doorInGuid in dto.AreaInDoors)
            {
                  // var doorIn = await door.GetAsync(doorInGuid,ct);
                  // dev = await door.GetCommandDetailAsync(doorInGuid,ct);
                  // await adapter.GetAdapter(dev.Vendor).Door.AddDoorsAsync(
                  //       dev.DeviceMac,
                  //       dev.DeviceIp,
                  //       doorIn.Type,
                  //       dev.DeviceExternalId,
                  //       [dev.ComponentExternalId ?? -1,dev.AltrComponentExternalId ?? -1],
                  //       doorIn.Metadata,
                  //       dto,
                  // );
            }

            // Send Door Out Command 
            foreach(var doorOutGuids in dto.AreaInDoors)
            {
                  
            }

            await Repo.AddAsync(d,ct);

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

      public async Task<Guid> UpdateAsync(UpdateAreaDto dto, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}