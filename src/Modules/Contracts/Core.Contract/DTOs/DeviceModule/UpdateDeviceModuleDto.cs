using SharedKernel.Enums;

namespace Core.Contract.DTOs.DeviceModule;

public sealed record UpdateDeviceModuleDto(
      Guid Guid,
      string Name,
      string SerialNumber,
      string Firmware,
      string Mac,
      string Port,
      string Address,
      DeviceModuleModel Model,
      int ReaderSlot,
      int OutputSlot,
      int InputSlot,
      Guid DeviceGuid,
      Guid LocationGuid
);