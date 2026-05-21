using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
using Microsoft.AspNetCore.Mvc;
using Output.Application.Interfaces;
using Output.Contract.DTOs;
using Output.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Output.Application.Behaviors;

public sealed class OutputBehavior(IOutputRepository repo,IAdapterFactory factory,IMessageBus bus) : IOutput
{
      public async Task<OutputDto> CreateAsync(CreateOutputDto dto)
      {

            var domain = new Outputs(
                  0,
                  await repo.GetLowestOutputComponentIdByMacAsync(dto.Mac),
                  dto.Mac,
                  dto.Name,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.OutputNo,
                  dto.Model,
                  dto.RelayMode,
                  dto.Type,
                  dto.LocationId,
                  dto.DefaultPulse,
                  true
                  );

             await factory.GetAdapter(dto.Type).Control.CreateAsync(
                              domain.Mac,
                              domain.ComponentId,
                              domain.DeviceComponentId,
                              domain.ModuleComponentId,
                              domain.OutputNo,
                              domain.Mode,
                              domain.DefaultPulse
                              );  

            var res = await repo.CreateAsync(domain);

            return res;
      }

      public async Task<OutputDto> DeleteByIdAsync(int id)
      {
            var res = await repo.GetByIdAsync(id);
            
            if(res.Id == 0)
                  throw new BadRequestException(MessageHelper.Output.OutputIdNotFound(id));

           await factory.GetAdapter(res.Type).Control.DeleteAsync(res.Mac,res.ComponentId,res.ComponentId,res.OutputNo,res.DefaultPulse);

            return await repo.DeleteByIdAsync(id);
            
      }

      public async Task<IEnumerable<short>> GetAvailalbleOutputByModuleIdAsync(int ModuleId)
      {
            // Query model type by moduleid
            var model = await bus.QueryAsync(new ModelByModuleIdQuery(ModuleId));
            var nOutput = AeroModuleModelHelper.nOutputByModel(EnumHelper.ToEnum<SioModel>(model));
            var outputList = Enumerable.Range(0, nOutput).Select(x => (short)x).ToArray();
            var res = await repo.GetUnavailableOutputByModuleIdAsync(ModuleId);
            return outputList.Except(res);
      }



      public async Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param)
      {
            var res = await repo.GetPaginationAsync(param);
            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetRelayModeAsync(string Type)
      {
            return await factory.GetAdapter(Type).Control.GetRelayModeAsync();
      }

      public async Task<BaseResponse> TriggerOutputAsync(int id, short Command)
      {
            // Check that any output with id
            if(!await repo.IsAnyWithIdAsync(id))
                  throw new BadRequestException(MessageHelper.Output.OutputIdNotFound(id));

            var output = await repo.GetByIdAsync(id);

            await factory.GetAdapter(output.Type).Control.TriggerOutputAsync(output.Mac,output.DeviceComponentId,output.ComponentId,Command);
            
                  
            
            return new BaseResponse(System.Net.HttpStatusCode.OK,MessageHelper.Common.Success,DateTime.UtcNow);
      }

      public async Task<OutputDto> UpdateAsync(OutputDto dto)
      {
            var res = await repo.GetByIdAsync(dto.Id);
            if(res.Id == 0)
                  throw new BadRequestException(MessageHelper.Common.RecordNotFound);

            var domain = new Outputs(
                  0,
                  dto.ComponentId,
                  dto.Mac,
                  dto.Name,
                  dto.DeviceComponentId,
                  dto.ModuleComponentId,
                  dto.OutputNo,
                  dto.Model,
                  dto.RelayMode,
                  dto.Type,
                  dto.LocationId,
                  dto.DefaultPulse,
                  dto.IsActive
                  );

           await factory.GetAdapter(domain.Type).Control.UpdateAsync(domain.Mac,domain.ComponentId,domain.ComponentId,domain.ModuleComponentId,domain.OutputNo,domain.Mode,domain.DefaultPulse);

            return await repo.UpdateAsync(domain);
      }
}