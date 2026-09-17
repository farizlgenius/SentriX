using Setting.Contract.DTOs.Setting;

namespace Setting.Contract.Interfaces;

public interface ISetting
{
  Task<AeroDriverSettingDto> GetAeroDriverSettingAsync(CancellationToken ct = default);

}