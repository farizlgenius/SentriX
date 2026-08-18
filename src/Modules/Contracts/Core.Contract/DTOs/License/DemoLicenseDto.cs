namespace Core.Contract.DTOs.License;

public sealed record DemoLicenseDto(
      // string BackendId,
      string Customer,
      string EndUser,
      string MachineId
      // string KeyId,
      // string EcdsaPublicKey,
      // string EcdhPublicKey
      );

