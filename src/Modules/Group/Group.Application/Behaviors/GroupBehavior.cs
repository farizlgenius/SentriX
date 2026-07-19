using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
using Group.Application.Interfaces;
using Group.Contract.DTOs;
using Group.Contract.Interfaces;
using Group.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Group.Application.Behaviors;

public sealed class GroupBehavior(IGroupRepository repo,IAdapterFactory factory,IMessageBus bus) : IGroup
{
      public async Task<GroupDto> CreateAsync(CreateGroupDto dto)
      {
            short ComponentId = await repo.GetLowestGroupComponentIdAsync();

            var d = new Groups(
                  Guid.NewGuid(),
                  ComponentId,
                  dto.Name,
                  dto.Doors.Select(x => new GroupDoor(
                        x.Mac,
                        x.Type,
                        x.DeviceComponentId,
                        x.DoorComponentId,
                        x.TimezoneComponentId
                  )).ToList(),
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
                  );


            foreach(var dd in d.GroupDoors)
            {
                  var DeviceComponentId = await bus.QueryAsync(new ComponentIdByMacQuery(dd.Mac));
                  var Doors =  d.GroupDoors.Select(x => new
                        {
                              x.Mac,
                              x.Type,
                              x.DeviceComponentId,
                              x.DoorComponentId,
                              x.TimeZoneComponentId
                        }).ToList();


                  await factory.GetAdapter(dd.Type).Group.CreateGroup(
                        d.Name,
                        d.ComponentId,
                        Doors.Select(x => (x.Mac,x.DeviceComponentId,x.DoorComponentId,x.TimeZoneComponentId)).ToList()
                        );
            }

            await repo.CreateAsync(d);

            return new GroupDto(
                  d.Guid,
                  d.ComponentId,
                  d.Name,
                  d.GroupDoors.Select(x => new GroupDoorDto(
                        x.Mac,
                        x.DeviceComponentId,
                        x.DoorComponentId,
                        x.TimeZoneComponentId,
                        x.Type
                  )).ToList(),
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );
            
      }

      public async Task<GroupDto> DeleteAsync(int id)
      {
            var d = await repo.GetByIdAsync(id);

            if(d == null)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Group", id));

            foreach(var data in d.Doors)
            {
                  await factory.GetAdapter(data.Type).Group.DeleteGroup(
                        data.Mac,
                        data.DeviceComponentId,
                        d.ComponentId
                        );
            }

            await repo.DeleteAsync(id);

            return d;

            
      }

      public async Task<GroupDto> DeleteByGuidAsync(Guid guid)
      {
            var d = await repo.GetByGuidAsync(guid);

            if(d == null)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Group", d.Guid.ToString()));

            foreach(var data in d.Doors)
            {
                  await factory.GetAdapter(data.Type).Group.DeleteGroup(
                        data.Mac,
                        data.DeviceComponentId,
                        d.ComponentId
                        );
            }

            await repo.DeleteAsync(guid);

            return d;

      }

      public async Task<IEnumerable<GroupDto>> GetByLocationIdAsync(int location)
      {
            return await repo.GetByLocationIdAsync(location);
      }

      public async Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param)
      {
            return await repo.GetPaginationAsync(param);
      }

      public async Task<GroupDto> UpdateAsync(GroupDto dto)
      {
            var entity = await repo.IsAnyByGuidAsync(dto.Guid);

            if(!entity)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Group", dto.Guid.ToString()));


            var d = new Groups(
                  dto.Guid,
                  dto.ComponentId,
                  dto.Name,
                  dto.Doors.Select(x => new GroupDoor(
                        x.Mac,
                        x.Type,
                        x.DeviceComponentId,
                        x.DoorComponentId,
                        x.TimezoneComponentId
                  )).ToList(),
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
                  );


            foreach(var dd in d.GroupDoors)
            {
                  var DeviceComponentId = await bus.QueryAsync(new ComponentIdByMacQuery(dd.Mac));
                  await factory.GetAdapter(dd.Type).Group.CreateGroup(
                        d.Name,
                        d.ComponentId,
                        d.GroupDoors.Select(x => (x.Mac,x.DeviceComponentId,x.DoorComponentId,x.TimeZoneComponentId)).ToList()
                        );
            }

            await repo.UpdateAsync(d);

            return new GroupDto(
                  d.Guid,
                  d.ComponentId,
                  d.Name,
                  d.GroupDoors.Select(x => new GroupDoorDto(
                        x.Mac,
                        x.DeviceComponentId,
                        x.DoorComponentId,
                        x.TimeZoneComponentId,
                        x.Type
                  )).ToList(),
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );;
      }
}