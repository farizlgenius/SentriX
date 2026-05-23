using Adapter.Abstraction.Interfaces;
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
            // Check first that mac is available 
            var deviceFlag = await bus.QueryAsync(new IsAnyWithMacQuery(dto.Mac));
            if(!deviceFlag)
                  throw new BadRequestException(MessageHelper.Device.DeviceMacNotFound(dto.Mac));

            short FirstComponentId = await repo.GetLowestDoorComponentIdWithExceptionAsync(dto.Mac,[]);
            short SecondComponentId = -1;

            if (dto.DoorType.Equals(DoorType.DUAL))
            {
                  SecondComponentId = await repo.GetLowestDoorComponentIdWithExceptionAsync(dto.Mac,[FirstComponentId]);
            }
            

            
            var domain = new Doors(
                  0,
                  dto.DeviceComponentId,
                  dto.Mac,
                  FirstComponentId,
                  SecondComponentId,
                  dto.Name,
                  dto.DoorType,
                  dto.Metadata,
                  dto.Type,
                  dto.LocationId,
                  dto.IsActive);


            await factory.GetAdapter(dto.Type).Door.CreateUpdateDoorAsync(
                  dto.Mac,
                  dto.DeviceComponentId,
                  dto.Metadata,
                  FirstComponentId,
                  SecondComponentId
                  );

            

            return await repo.CreateAsync(domain);
      }

      public async Task<DoorDto> DeleteAsync(int id)
      {
            var entity = await repo.GetByIdAsync(id);
            if(entity == null)
                  throw new BadRequestException(MessageHelper.Door.DoorIdNotFound(id));

            
            await factory.GetAdapter(entity.Type).Door.DeleteDoorAsync(
                  entity.Mac,
                  entity.DeviceComponentId,
                  entity.Metadata,
                  entity.ComponentId,
                  entity.SecondComponentId
                  );

            return await repo.DeleteAsync(entity.Id);

            
      }

      public async Task<Pagination<DoorDto>> GetDoorPaginationAsync(PaginationParams param)
      {
            var res = await repo.GetDoorPaginationAsync(param);
            return res;
      }

      public async Task<DoorDto> UpdateAsync(DoorDto dto)
      {
            if(!await repo.IsAnyByIdAsync(dto.Id))
                  throw new BadRequestException(MessageHelper.Door.DoorIdNotFound(dto.Id));

            short SecondComponentId = -1;
            if (dto.DoorType.Equals(DoorType.DUAL) && dto.SecondComponentId == -1)
            {
                  SecondComponentId = await repo.GetLowestDoorComponentIdWithExceptionAsync(dto.Mac,[]);
            }
            

            
            var domain = new Doors(
                  0,
                  dto.DeviceComponentId,
                  dto.Mac,
                  dto.ComponentId,
                  dto.SecondComponentId == -1 ? SecondComponentId : dto.SecondComponentId,
                  dto.Name,
                  dto.DoorType,
                  dto.Metadata,
                  dto.Type,
                  dto.LocationId,
                  dto.IsActive);


            await factory.GetAdapter(dto.Type).Door.CreateUpdateDoorAsync(
                  dto.Mac,
                  dto.DeviceComponentId,
                  dto.Metadata,
                  dto.ComponentId,
                  SecondComponentId
                  );

            

            return await repo.UpdateAsync(domain);

            
      }
}