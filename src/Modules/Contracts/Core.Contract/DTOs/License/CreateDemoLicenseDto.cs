namespace Core.Contract.DTOs.License;

public sealed record CreateDemoLicenseDto(
      Guid BackendGuid,
      Guid KeyGuid,
      string MachineId
      );

