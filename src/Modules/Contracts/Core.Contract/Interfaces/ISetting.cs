using Core.Contract.DTOs.Setting;

namespace Core.Contract.Interfaces;

public interface ISetting
{
  Task<AeroDriverSettingDto> GetAeroDriverSettingAsync();

}