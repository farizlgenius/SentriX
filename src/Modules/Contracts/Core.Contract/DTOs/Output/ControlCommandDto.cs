using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record ControlCommandDto(
      Guid Guid,
      ControlCommand Command,
      short OnTime,
      short OffTime,
      short Repeat=0
);