using Setting.Contract.DTOs;
using Setting.Contract.DTOs.Setting;
using SharedKernel.Domain;

namespace Setting.Contract.Interfaces;

public interface ISetting
{
  Task<AeroDriverSettingDto> GetAeroDriverSettingAsync(CancellationToken ct = default);

}