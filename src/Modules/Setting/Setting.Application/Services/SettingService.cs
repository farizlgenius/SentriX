using Setting.Application.Interfaces;
using Setting.Contract.DTOs;
using Setting.Contract.DTOs.Setting;
using Setting.Contract.Interfaces;
using SharedKernel.Domain;

namespace Setting.Application.Services;

public sealed class SettingService(ISettingRepository repo) : ISetting
{
  public async Task<AeroDriverSettingDto> GetAeroDriverSettingAsync(CancellationToken ct = default)
  {
    return await repo.GetAeroDriverSettingAsync(ct);
  }


}