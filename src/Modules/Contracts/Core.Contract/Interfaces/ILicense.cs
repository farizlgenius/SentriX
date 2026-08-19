using Core.Contract.DTOs.License;

namespace Core.Contract.Interfaces;

public interface ILicense
{
      Task<string> GetMachineIdAsync(CancellationToken ct = default);
      Task<object> RequestDemoAsync(DemoLicenseDto dto, CancellationToken ct = default);
      Task<bool> DownloadAsync(DownloadLicenseDto dto, CancellationToken ct = default);
      Task<bool> ActivateAsync(ActivateDto dto, CancellationToken ct = default);
}