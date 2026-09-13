using Core.Application.Interfaces;
using Core.Contract.DTOs.Setting;
using Core.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class SettingService(ISettingRepository repo) : ISetting
{
  public async Task<AeroDriverSettingDto> GetAeroDriverSettingAsync()
  {
    return await repo.GetAeroDriverSettingAsync();
  }
}