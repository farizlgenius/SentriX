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
                  dto.Metadata,
                  dto.LocationId,
                  dto.IsActive
                  );

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(dto.LocationId));

            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Group.CreateUpdateLevel(
                        data.Mac,
                        data.ComponentId,
                        ComponentId,
                        dto.Metadata
                        );
            }

            return await repo.CreateAsync(domain);
            
      }

      public async Task<GroupDto> DeleteAsync(int id)
      {
            var entity = await repo.GetByIdAsync(id);

            if(entity == null)
                  throw new BadRequestException(MessageHelper.Group.GroupIdNotFound(id));

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

      public async Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param)
      {
            throw new NotImplementedException();
      }

      public async Task<GroupDto> UpdateAsync(GroupDto dto)
      {
            var entity = await repo.GetByIdAsync(dto.Id);

            if(entity == null)
                  throw new BadRequestException(MessageHelper.Group.GroupIdNotFound(dto.Id));

            var domain = new Groups(
                  dto.Id,
                  dto.ComponentId,
                  dto.Name,
                  dto.Metadata,
                  dto.LocationId,
                  dto.IsActive
                  );

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(entity.LocationId));

            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Group.CreateUpdateLevel(
                        data.Mac,
                        data.ComponentId,
                        domain.ComponentId,
                        domain.Metadata
                        );
            }

            return await repo.UpdateAsync(domain);
      }
}