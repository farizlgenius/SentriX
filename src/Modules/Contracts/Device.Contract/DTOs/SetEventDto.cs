namespace Device.Contract.DTOs;

public sealed record SetEventDto(
      string Type,
      int DeviceId,
      bool IsEnable
      );