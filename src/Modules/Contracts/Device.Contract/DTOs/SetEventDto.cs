namespace Device.Contract.DTOs;

public sealed record SetEventDto(
      Guid DeviceGuid,
      bool IsEnable
      );