using Core.Contract.DTOs.Setting;

namespace Core.Application.Interfaces;

public interface ISettingRepository
{
  Task<AeroDriverSettingDto> GetAeroDriverSettingAsync();
}