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

            var domain = new Groups(
                  0,
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
                  dto.IsActive
                  );


            foreach(var d in domain.GroupDoors)
            {
                  var DeviceComponentId = await bus.QueryAsync(new ComponentIdByMacQuery(d.Mac));
                  await factory.GetAdapter(d.Type).Group.CreateUpdateLevel(
                        d.Mac,
                        (short)DeviceComponentId,
                        ComponentId,
                        d.DoorDetails.Select(x => (x.DoorComponentId,x.TimezoneComponentId)).ToList()
                        );
            }

            return await repo.CreateAsync(domain);
            
      }

      public async Task<GroupDto> DeleteAsync(int id)
      {
            var entity = await repo.GetByIdAsync(id);

            if(entity == null)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Group", id));

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(entity.LocationId));

            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Group.DeleteLevel(
                        data.Mac,
                        data.ComponentId,
                        entity.ComponentId
                        );
            }

            return await repo.DeleteAsync(id);

            
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
            var entity = await repo.GetByIdAsync(dto.Id);

            if(entity == null)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Group", dto.Id));

            // var domain = new Groups(
            //       dto.Id,
            //       dto.ComponentId,
            //       dto.Name,
            //       dto.Metadata,
            //       dto.LocationId,
            //       dto.IsActive
            //       );

            var domain = new Groups(
                  0,
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
                  dto.IsActive
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

            return await repo.UpdateAsync(domain);
      }
}