using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
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
                  dto.Name,
                  dto.ModuleId,
                  dto.OutputNo,
                  dto.Model,
                  dto.LocationId,
                  dto.DefaultPulse
                  );

            await factory.GetAdapter(Venders.AERO).Control.CreateAsync(dto);

            var res = await repo.CreateAsync(domain);

            return res;
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

      public async Task<IEnumerable<OptionDto>> GetRelayModeAsync()
      {
            var res = await factory.GetAdapter(Venders.AERO).Control.GetRelayModeAsync();
            return res;
      }

      public async Task<BaseResponse> TriggerOutputAsync(int id, int Command)
      {
            // Check that any output with id
            if(!await repo.IsAnyWithIdAsync(id))
                  throw new BadRequestException(MessageHelper.Output.OutputIdNotFound(id));

            var output = await repo.GetByIdAsync(id);
                  
            await factory.GetAdapter(Venders.AERO).Control.TriggerOutputAsync(output.ModuleId,Command);
            return new BaseResponse(System.Net.HttpStatusCode.OK,MessageHelper.Common.Success,DateTime.UtcNow);
      }
}