using Core.Contract.DTOs.License;

namespace Core.Contract.Interfaces;

public interface ILicense
{
      Task<MachineIdDto> GetMachineIdAsync(CancellationToken ct = default);
      Task<bool> CheckLicenseAsync(CancellationToken ct = default);
      Task<bool> GenerateDemoAsync(CreateDemoLicenseDto dto,CancellationToken ct = default);
      Task<bool> GenerateLicenseAsync(CancellationToken ct = default);
}