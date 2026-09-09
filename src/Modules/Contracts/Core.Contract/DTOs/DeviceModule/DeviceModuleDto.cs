using SharedKernel.Enums;

namespace Core.Contract.DTOs.DeviceModule;

public sealed record DeviceModuleDto(
      Guid Guid,
      string Name,
      string SerialNumber,
      string Firmware,
      string Mac,
      int Port,
      int Address,
      DeviceModuleModel Model,
      int ReaderSlot,
      int OutputSlot,
      int InputSlot,
      Guid DeviceGuid,
      string DeviceName,
      Guid LocationGuid,
      string LocationName,
      bool IsActive,
      bool IsDefault
);