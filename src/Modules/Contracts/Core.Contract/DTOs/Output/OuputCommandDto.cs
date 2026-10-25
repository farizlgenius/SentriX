using SharedKernel.Enums;

namespace Core.Contract.DTOs.Output;

public sealed record OutputCommandDto(
      Guid Guid,
      OutputCommand Command,
      short OnTime,
      short OffTime,
      short Repeat=0
);