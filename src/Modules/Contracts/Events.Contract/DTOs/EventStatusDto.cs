namespace Events.Contract.DTOs;

public sealed record EventStatusDto(
      int DeviceId,
      bool IsEnable
      );