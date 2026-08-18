using Core.Contract.DTOs.License;

namespace Core.Contract.Interfaces;

public interface ILicense
{
      Task<bool> RequestDemoAsync(CreateDemoLicenseDto dto,CancellationToken ct = default);
}