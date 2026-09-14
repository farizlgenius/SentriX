using Setting.Contract.DTOs.Setting;

namespace Setting.Application.Interfaces;

public interface ISettingRepository
{
  Task<AeroDriverSettingDto> GetAeroDriverSettingAsync();
}