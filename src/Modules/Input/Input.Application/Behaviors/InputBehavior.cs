using System.Net;
using Adapter.Abstraction.Interfaces;
using Input.Application.Interfaces;
using Input.Contract.DTOs;
using Input.Contract.Interfaces;
using Input.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Input.Application.Behaviors;

public sealed class InputBehavior(IInputRepository repo,IAdapterFactory factory) : IInput
{
      public async Task<InputDto> CreateInputAsync(CreateInputDto dto)
      {
            if(string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.NameEmpty);

            if(string.IsNullOrWhiteSpace(dto.Mac))
                  throw new BadRequestException(MessageHelper.Common.MacEmpty);

            var componentId = await repo.GetLowestInputComponentIdAsync(dto.Mac);

            var domain = new Inputs(
                  0,
                  componentId,
                  dto.Name,
                  dto.Mac,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.Metadata,
                  dto.LocationId,
                  dto.IsActive
                  );

            await factory.GetAdapter(dto.Type).Monitor.CreateUpdateMonitorPoint(
                  dto.Mac,
                  dto.ComponentId,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.InputNo,
                  dto.Metadata
                  );

            return await repo.CreateInputAsync(domain);

            
      }

      public async Task<InputGroupDto> CreateInputGroupAsync(CreateInputGroupDto dto)
      {
            if(string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.NameEmpty);

            if(string.IsNullOrWhiteSpace(dto.Mac))
                  throw new BadRequestException(MessageHelper.Common.MacEmpty);

            var componentId = await repo.GetLowestInputGroupComponentIdAsync(dto.Mac);

            var domain = new InputGroups(
                  0,
                  componentId,
                  dto.Name,
                  dto.Mac,
                  dto.DeviceComponentId,
                  dto.Metadata,
                  dto.LocationId,
                  dto.IsActive
                  );

            await factory.GetAdapter(dto.Type).Monitor.CreateUpdateMonitorGroup(
                  domain.Mac,
                  domain.DeviceComponentId,
                  domain.ComponentId,
                  domain.Metadata
            );

            return await repo.CreateInputGroupAsync(domain);
      }

      public async Task<InputDto> DeleteInputAsync(int id)
      {
            var entity = await repo.GetByIdAsync(id);

            if(entity.Id == 0)
                  throw new BadRequestException(MessageHelper.Input.InputIdNotFound(id));

            await factory.GetAdapter(entity.Type).Monitor.DeleteMonitorPoint(
                  entity.Mac,
                  entity.ComponentId,
                  entity.DeviceComponentId,
                  entity.InputNo,
                  entity.Metadata
                  );

            return await repo.DeleteInputAsync(id);
      }

      public async Task<InputGroupDto> DeleteInputGroupAsync(int id)
      {
            var entity = await repo.GetGroupByIdAsync(id);

            if(entity.Id == 0)
                  throw new BadRequestException(MessageHelper.Input.GroupIdNotFound(id));

            await factory.GetAdapter(entity.Type).Monitor.DeleteMonitorGroup(
                  entity.Mac,
                  entity.ComponentId,
                  entity.DeviceComponentId);

            return await repo.DeleteInputGroupAsync(id);
      }

      public async Task<Pagination<InputGroupDto>> GetGroupPaginationAsync(PaginationParams param)
      {
            var res = await repo.GetInputGroupPaginationAsync(param);
            return res;
      }

      public async Task<Pagination<InputDto>> GetInputPagination(PaginationParams param)
      {
            return await repo.GetInputPaginationAsync(param);
      }

      public async Task<BaseResponse> InputMaskAsync(int id,bool IsMask)
      {
            var entity = await repo.GetByIdAsync(id);

            if(entity.Id == 0)
                  throw new BadRequestException(MessageHelper.Input.InputIdNotFound(id));

            await factory.GetAdapter(entity.Type).Monitor.MaskMonitorPoint(
                  entity.Mac,
                  entity.DeviceComponentId,
                  entity.ComponentId,
                  IsMask
            );

            return new BaseResponse(HttpStatusCode.OK,MessageHelper.Common.Success,DateTime.UtcNow);
      }

      public async Task<InputDto> UpdateInputAsync(InputDto dto)
      {
            if(string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.NameEmpty);

            if(string.IsNullOrWhiteSpace(dto.Mac))
                  throw new BadRequestException(MessageHelper.Common.MacEmpty);


            var domain = new Inputs(
                  dto.Id,
                  dto.ComponentId,
                  dto.Name,
                  dto.Mac,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.Metadata,
                  dto.LocationId,
                  dto.IsActive
                  );

            await factory.GetAdapter(dto.Type).Monitor.CreateUpdateMonitorPoint(
                  dto.Mac,
                  dto.ComponentId,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.InputNo,
                  dto.Metadata
                  );

            return await repo.UpdateInputAsync(domain);
      }

      public async Task<InputGroupDto> UpdateInputGroupAsync(InputGroupDto dto)
      {
            if(string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.NameEmpty);

            if(string.IsNullOrWhiteSpace(dto.Mac))
                  throw new BadRequestException(MessageHelper.Common.MacEmpty);

            var domain = new InputGroups(
                  dto.Id,
                  dto.ComponentId,
                  dto.Name,
                  dto.Mac,
                  dto.DeviceComponentId,
                  dto.Metadata,
                  dto.LocationId,
                  dto.IsActive
                  );

            await factory.GetAdapter(dto.Type).Monitor.CreateUpdateMonitorGroup(
                  domain.Mac,
                  domain.DeviceComponentId,
                  domain.ComponentId,
                  domain.Metadata
            );

            return await repo.UpdateInputGroupAsync(domain);
      }
}