namespace Output.Contract.DTOs;

public sealed record CreateOutputDto(
      string Name,
      int ModuleId,
      short OutputNo,
      string Model,
      short RelayMode,
      int LocationId,
      short DefaultPulse
      );