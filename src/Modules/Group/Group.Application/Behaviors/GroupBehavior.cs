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
                  dto.Doors.GroupBy(
                        x => (x.Mac,x.Type),
                        x => (x.DoorComponentId,x.TimezoneComponentId)
                  ).Select(gp => (
                        gp.Key.Mac,
                        gp.Key.Type,
                        gp.ToList()
                  )).ToList(),
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
                  );


            foreach(var dd in d.GroupDoors)
            {
                  var DeviceComponentId = await bus.QueryAsync(new ComponentIdByMacQuery(dd.Mac));
                  await factory.GetAdapter(dd.Type).Group.CreateUpdateLevel(
                        dd.Mac,
                        (short)DeviceComponentId,
                        ComponentId,
                        dd.DoorDetails.Select(x => (x.DoorComponentId,x.TimezoneComponentId)).ToList()
                        );
            }

            await repo.CreateAsync(d);

            return new GroupDto(
                  d.Guid,
                  d.ComponentId,
                  d.Name,
                  d.GroupDoors.SelectMany(x => x.DoorDetails.Select(s => new GroupDoorDto(
                        x.Mac,
                        s.DoorComponentId,
                        s.TimezoneComponentId,
                        x.Type
                  ))).ToList(),
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

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(d.LocationId));

            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Group.DeleteLevel(
                        data.Mac,
                        data.ComponentId,
                        d.ComponentId
                        );
            }

            await repo.DeleteAsync(id);

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
            var entity = await repo.GetByGuidAsync(dto.Guid);

            if(entity == null)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Group", dto.Guid.ToString()));


            var domain = new Groups(
                  dto.Guid,
                  dto.ComponentId,
                  dto.Name,
                  dto.Doors.GroupBy(
                        x => (x.Mac,x.Type),
                        x => (x.DoorComponentId,x.TimezoneComponentId)
                  ).Select(gp => (
                        gp.Key.Mac,
                        gp.Key.Type,
                        gp.ToList()
                  )).ToList(),
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
                  );

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(entity.LocationId));

            foreach(var d in domain.GroupDoors)
            {
                  var DeviceComponentId = await bus.QueryAsync(new ComponentIdByMacQuery(d.Mac));
                  await factory.GetAdapter(d.Type).Group.CreateUpdateLevel(
                        d.Mac,
                        (short)DeviceComponentId,
                        dto.ComponentId,
                        d.DoorDetails.Select(x => (x.DoorComponentId,x.TimezoneComponentId)).ToList()
                        );
            }

            await repo.UpdateAsync(domain);

            return entity;
      }
}