using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;
using Output.Application.Interfaces;
using Output.Contract.DTOs;
using Output.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Output.Application.Behaviors;

public sealed class OutputBehavior(IOutputRepository repo,IAdapterFactory factory) : IOutput
{
      public async Task<OutputDto> CreateAsync(CreateOutputDto dto)
      {
            var domain = new Outputs(
                  dto.Name,
                  dto.ModuleId,
                  dto.LocationId,
                  dto.Metadata
                  );
            var res = await repo.CreateAsync(domain);

            await factory.GetAdapter(Venders.AERO).Control.CreateAsync(dto);

            return res;
      }

      public async Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param)
      {
            var res = await repo.GetPaginationAsync(param);
            return res;
      }
}