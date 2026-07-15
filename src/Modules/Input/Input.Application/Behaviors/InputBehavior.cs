using System.Net;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
using Input.Application.Interfaces;
using Input.Contract.DTOs;
using Input.Contract.Interfaces;
using Input.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Input.Application.Behaviors;

public sealed class InputBehavior(IInputRepository repo,IAdapterFactory factory,IMessageBus bus) : IInput
{
      public async Task<InputDto> CreateInputAsync(CreateInputDto dto)
      {
            if(string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(dto.Name)));

            if(string.IsNullOrWhiteSpace(dto.Mac))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(dto.Mac)));

            var componentId = await repo.GetLowestInputComponentIdAsync(dto.Mac);

            var domain = new Inputs(
                  0,
                  componentId,
                  dto.Name,
                  dto.Mac,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.InputNo,
                  dto.SensorMode,
                  dto.Debounce,
                  dto.HoldTime,
                  dto.LogFunction,
                  dto.LatchMode,
                  dto.DelayEntry,
                  dto.DelayExit,
                  dto.Type,
                  dto.LocationId,
                  dto.IsActive
                  );

            await factory.GetAdapter(dto.Type).Monitor.CreateUpdateMonitorPoint(
                  dto.Mac,
                  dto.ComponentId,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.InputNo,
                  dto.SensorMode,
                  dto.Debounce,
                  dto.HoldTime,
                  dto.LogFunction,
                  dto.LatchMode,
                  dto.DelayEntry,
                  dto.DelayExit
                  );

            return await repo.CreateInputAsync(domain);

            
      }

      public async Task<InputGroupDto> CreateInputGroupAsync(CreateInputGroupDto dto)
      {
            if(string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(dto.Name)));

            var componentId = await repo.GetLowestInputGroupComponentIdAsync();

            var domain = new InputGroups(
                  0,
                  componentId,
                  dto.Name,
                  dto.Type,
                  dto.InputGroupDetailDtos
                  .GroupBy(
                        k => (k.Mac,k.DeviceComponentId)
                  )
                  .Select(x => new InputGroupDetail(
                        0,
                        x.Key.Mac,
                        x.Key.DeviceComponentId,
                        x.Select(i => new InputList(
                              0,
                              i.InputType,
                              i.InputComponentId,
                              dto.LocationId,
                              dto.IsActive
                        )).ToList(),
                        dto.LocationId,
                        dto.IsActive
                  )).ToList(),
                  dto.LocationId,
                  dto.IsActive
                  );

            foreach(var d in domain.InputGroupDetails)
            {
                  await factory.GetAdapter(dto.Type).Monitor.CreateUpdateMonitorGroup(
                        d.Mac,
                        d.DeviceComponentId,
                        componentId,
                        d.InputList.Select(x => (x.Type, x.Number)).ToList()
                  );
            }

            

            return await repo.CreateInputGroupAsync(domain);
      }

      public async Task<InputDto> DeleteInputAsync(int id)
      {
            var entity = await repo.GetByIdAsync(id);

            if(entity.Id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Input", id));

            await factory.GetAdapter(entity.Type).Monitor.DeleteMonitorPoint(
                  entity.Mac,
                  entity.ComponentId,
                  entity.DeviceComponentId,
                  entity.InputNo,
                  entity.SensorMode,
                  entity.Debounce,
                  entity.HoldTime,
                  entity.LogFunction,
                  entity.LatchMode,
                  entity.DelayEntry,
                  entity.DelayExit
                  );

            return await repo.DeleteInputAsync(id);
      }

      public async Task<InputGroupDto> DeleteInputGroupAsync(int id)
      {
            var entity = await repo.GetGroupByIdAsync(id);

            if(entity.Id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Input Group", id));



            foreach(var d in entity.InputGroupDetailDtos)
            {
                  await factory.GetAdapter(entity.Type).Monitor.CreateUpdateMonitorGroup(
                        d.Mac,
                        d.DeviceComponentId,
                        entity.ComponentId,
                        new List<(short Type, short Number)>()
                  );
            }

            return await repo.DeleteInputGroupAsync(id);
      }

      public async Task<IEnumerable<short>> GetAvailalbleInputByModuleIdAsync(int id, CancellationToken ct = default)
      {
            // Query model type by moduleid
            var model = await bus.QueryAsync(new ModelByModuleIdQuery(id));
            var nInput = AeroModuleModelHelper.nInputByModel(EnumHelper.ToEnum<SioModel>(model));
            var inputList = Enumerable.Range(0, nInput).Select(x => (short)x).ToArray();
            var res = await repo.GetUnavailableInputByModuleIdAsync(id);
            return inputList.Except(res);
      }

      public async Task<Pagination<InputGroupDto>> GetGroupPaginationAsync(PaginationParams param)
      {
            var res = await repo.GetInputGroupPaginationAsync(param);
            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetInputModeAsync()
      {
            var res = await repo.GetInputModeAsync();
            return res;
      }

      public async Task<Pagination<InputDto>> GetInputPagination(PaginationParams param)
      {
            return await repo.GetInputPaginationAsync(param);
      }

      public async Task InputMaskAsync(int id,bool IsMask)
      {
            var entity = await repo.GetByIdAsync(id);

            if(entity.Id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Input", id));

            await factory.GetAdapter(entity.Type).Monitor.MaskMonitorPoint(
                  entity.Mac,
                  entity.DeviceComponentId,
                  entity.ComponentId,
                  IsMask
            );

      }

      public async Task<InputDto> UpdateInputAsync(InputDto dto)
      {
            if(string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(dto.Name)));

            if(string.IsNullOrWhiteSpace(dto.Mac))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(dto.Mac)));


            var domain = new Inputs(
                  dto.Id,
                  dto.ComponentId,
                  dto.Name,
                  dto.Mac,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.InputNo,
                  dto.SensorMode,
                  dto.Debounce,
                  dto.HoldTime,
                  dto.LogFunction,
                  dto.LatchMode,
                  dto.DelayEntry,
                  dto.DelayExit,
                  dto.Type,
                  dto.LocationId,
                  dto.IsActive
                  );

            await factory.GetAdapter(dto.Type).Monitor.CreateUpdateMonitorPoint(
                  dto.Mac,
                  dto.ComponentId,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.InputNo,
                  dto.SensorMode,
                  dto.Debounce,
                  dto.HoldTime,
                  dto.LogFunction,
                  dto.LatchMode,
                  dto.DelayEntry,
                  dto.DelayExit
                  );

            return await repo.UpdateInputAsync(domain);
      }

      public async Task<InputGroupDto> UpdateInputGroupAsync(InputGroupDto dto)
      {
            if(string.IsNullOrWhiteSpace(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.Empty(nameof(dto.Name)));


           var domain = new InputGroups(
                  0,
                  dto.ComponentId,
                  dto.Name,
                  dto.Type,
                  dto.InputGroupDetailDtos
                  .GroupBy(
                        k => (k.Mac,k.DeviceComponentId)
                  )
                  .Select(x => new InputGroupDetail(
                        0,
                        x.Key.Mac,
                        x.Key.DeviceComponentId,
                        x.Select(i => new InputList(
                              0,
                              i.InputType,
                              i.InputComponentId,
                              dto.LocationId,
                              dto.IsActive
                        )).ToList(),
                        dto.LocationId,
                        dto.IsActive
                  )).ToList(),
                  dto.LocationId,
                  dto.IsActive
                  );

            foreach(var d in domain.InputGroupDetails)
            {
                  await factory.GetAdapter(dto.Type).Monitor.CreateUpdateMonitorGroup(
                        d.Mac,
                        d.DeviceComponentId,
                        dto.ComponentId,
                        d.InputList.Select(x => (x.Type, x.Number)).ToList()
                  );
            }

            return await repo.UpdateInputGroupAsync(domain);
      }
}