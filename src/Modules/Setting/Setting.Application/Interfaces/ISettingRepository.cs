using Setting.Contract.DTOs;
using Setting.Contract.DTOs.Setting;
using SharedKernel.Domain;

namespace Setting.Application.Interfaces;

public interface ISettingRepository
{
  Task<AeroDriverSettingDto> GetAeroDriverSettingAsync(CancellationToken ct= default);

}