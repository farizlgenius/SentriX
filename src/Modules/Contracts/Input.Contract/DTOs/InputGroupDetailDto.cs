using SharedKernel.Domain;

namespace Input.Contract.DTOs;

public sealed record InputGroupDetailDto(
      string Mac,
      short DeviceComponentId,
      short InputType,
      short InputComponentId
);