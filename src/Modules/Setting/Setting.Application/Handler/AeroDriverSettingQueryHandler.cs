using Setting.Application.Interfaces;
using Setting.Contract.DTOs.Setting;
using Setting.Contract.Queries;
using SharedKernel.Messaging;

namespace Setting.Application.Handler;

public sealed class AeroDriverSettingQueryHandler(ISettingRepository repo) : IQueryHandler<AeroDriverSettingQuery, AeroDriverSettingDto>
{
      public async Task<AeroDriverSettingDto> HandleAsync(AeroDriverSettingQuery query, CancellationToken ct)
      {
            return await repo.GetAeroDriverSettingAsync();
      }
}