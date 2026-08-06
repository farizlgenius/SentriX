using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Command;
using Device.Contract.Queries;
using Door.Application.Interfaces;
using Door.Contract.DTOs;
using Door.Contract.Interfaces;
using Door.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Door.Application.Behaviors;

public sealed class DoorBehavior(IDoorRepository repo,IMessageBus bus,IAdapterFactory factory) : IDoor
{
      public async Task<DoorDto> CreateAsync(CreateDoorDto dto)
      {
            var deviceGuid = await bus.QueryAsync(new DeviceByMac(dto.Mac));

            var d = new Doors(
                  Guid.NewGuid(),
                  dto.Name,
                  dto.DoorType,
                  dto.Metadata,
                  dto.Type,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault);


            await factory.GetAdapter(dto.Type).Door.CreateAsync(,d.Guid,dto.Metadata);

            // Save component used


            DoorMetadata? data = JsonHelper.Deserialize<DoorMetadata>(dto.Metadata);

            // Redaer in
            if(data != null)
            {
                   await bus.SendAsync(new AddReaderUsedCommand(
                        data.ReaderIn.ReaderNumber,
                        data.ReaderIn.ReaderModuleGuid,
                        dto.LocationId
                  ));

                  await bus.SendAsync(
                        new AddReaderUsedCommand(
                              data.ReaderOut.ReaderNumber,
                              data.ReaderOut.ReaderModuleGuid,
                              dto.LocationId
                        )
                  );

                  await bus.SendAsync(
                        new AddReaderUsedCommand(
                              data.AltrReader.AltrRdrNumber,
                              data.AltrReader.AltrRdrModuleId,
                              dto.LocationId
                        )
                  );


                  await bus.SendAsync(
                        new AddRelayUsedCommand(
                              data.Relay.RelayNumber,
                              data.Relay.RelayModuleGuid,
                              dto.LocationId
                        )
                  );

                  await bus.SendAsync(
                        new AddInputUsedCommand(
                              data.Sensor.SensorNumber,
                              data.Sensor.SensorModuleGuid,
                              dto.LocationId
                        )
                  );

                  await bus.SendAsync(
                        new AddInputUsedCommand(
                              data.Rex.Rex0Number,
                              data.Rex.Rex0ModuleGuid,
                              dto.LocationId
                        )
                  );

                   await bus.SendAsync(
                        new AddInputUsedCommand(
                              data.Rex.Rex1Number,
                              data.Rex.Rex1ModuleGuid,
                              dto.LocationId
                        )
                  );
            } 
           

            await repo.AddAsync(d);

            return new DoorDto(
                  d.Guid,
                  d.ComponentId,
                  d.Name,
                  d.DeviceComponentId,
                  d.SecondComponentId,
                  d.Mac,
                  d.DoorType,
                  d.Metadata,
                  d.LocationId,
                  d.Type,
                  d.IsActive,
                  d.IsDefault
            );
      }

      public async Task<DoorDto> DeleteAsync(Guid guid)
      {
            var entity = await repo.GetByGuidAsync(guid);
            if(entity == null)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Door", guid.ToString()));

            
            await factory.GetAdapter(entity.Type).Door.DeleteDoorAsync(
                  entity.Mac,
                  entity.DeviceComponentId,
                  entity.Metadata,
                  entity.ComponentId,
                  entity.SecondComponentId
                  );

             DoorMetadata? data = JsonHelper.Deserialize<DoorMetadata>(entity.Metadata);

            // Redaer in
            if(data != null)
            {
                  await bus.SendAsync(new DeleteReaderUsedCommand(
                        data.ReaderIn.Guid
                  ));

                  await bus.SendAsync(
                        new DeleteReaderUsedCommand(
                              data.ReaderOut.Guid
                        )
                  );

                  await bus.SendAsync(
                        new DeleteReaderUsedCommand(
                              data.AltrReader.Guid
                        )
                  );


                  await bus.SendAsync(
                        new DeleteRelayUsedCommand(
                              data.Relay.Guid
                        )
                  );

                  await bus.SendAsync(
                        new DeleteInputUsedCommand(
                              data.Sensor.Guid
                        )
                  );

                  await bus.SendAsync(
                        new DeleteInputUsedCommand(
                              data.Rex.Guid
                        )
                  );

                   await bus.SendAsync(
                        new DeleteInputUsedCommand(
                              data.Rex.Guid
                        )
                  );
            } 

            await repo.DeleteAsync(entity.Guid);

            return new DoorDto(
                  entity.Guid,
                  entity.ComponentId,
                  entity.Name,
                  entity.DeviceComponentId,
                  entity.SecondComponentId,
                  entity.Mac,
                  entity.DoorType,
                  entity.Metadata,
                  entity.LocationId,
                  entity.Type,
                  entity.IsActive,
                  entity.IsDefault
            );

            
      }

      public async Task<IEnumerable<OptionDto>> GetAccessControlFlagAsync()
      {
            var res = await repo.GetAccessControlFlagAsync();
            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetSpareFlagAsync()
      {
            var res = await repo.GetSpareFlagAsync();
            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetApbModeAsync()
      {
            var res = await repo.GetApbModeAsync();
            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetDoorModeAsync()
      {
             var res = await repo.GetDoorModeAsync();
            return res;
      }

      public async Task<Pagination<DoorDto>> GetDoorPaginationAsync(PaginationParams param)
      {
            var res = await repo.GetDoorPaginationAsync(param);
            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetReaderModeAsync()
      {
            var res = await repo.GetReaderModeAsync();
            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetStrikeModeAsync()
      {
             var res = await repo.GetStrikeModeAsync();
            return res;
      }

      public async Task<DoorDto> UpdateAsync(DoorDto dto)
      {
            if(!await repo.IsAnyByGuidAsync(dto.Guid))
                  throw new BadRequestException(MessageHelper.Common.NotFound("Door", dto.Guid.ToString()));

            short SecondComponentId = -1;
            if (dto.DoorType.Equals(DoorType.Dual) && dto.SecondComponentId == -1)
            {
                  SecondComponentId = await repo.GetLowestDoorComponentIdWithExceptionAsync(dto.Mac,[]);
            }
            

            
            var d = new Doors(
                  dto.Guid,
                  dto.DeviceComponentId,
                  dto.Mac,
                  dto.ComponentId,
                  dto.SecondComponentId == -1 ? SecondComponentId : dto.SecondComponentId,
                  dto.Name,
                  dto.DoorType,
                  dto.Metadata,
                  dto.Type,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
                  );


            await factory.GetAdapter(dto.Type).Door.CreateUpdateDoorAsync(
                  dto.Mac,
                  dto.DeviceComponentId,
                  dto.Metadata,
                  dto.ComponentId,
                  SecondComponentId
                  );

            

            await repo.UpdateAsync(d);

            return new DoorDto(
                  d.Guid,
                  d.ComponentId,
                  d.Name,
                  d.DeviceComponentId,
                  d.SecondComponentId,
                  d.Mac,
                  d.DoorType,
                  d.Metadata,
                  d.LocationId,
                  d.Type,
                  d.IsActive,
                  d.IsDefault
            ); 

            
      }

      public async Task<IEnumerable<OptionDto>> GetOsdpBaudrateAsync()
      {
            return await repo.GetOsdpBaudrateAsync();
      }

      public async Task<IEnumerable<OptionDto>> GetDoorOptionByLocationIdAsync(int LocationId)
      {
            return await repo.GetDoorOptionByLocationIdAsync(LocationId);
      }

      public async Task<string> GetNameByMacAndComponentIdAsync(string Mac, short ComponentId, CancellationToken ct = default)
      {
            return await repo.GetNameByMacAndComponentIdAsync(Mac,ComponentId,ct);
      }
}