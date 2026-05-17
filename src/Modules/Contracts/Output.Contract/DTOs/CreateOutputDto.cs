namespace Output.Contract.DTOs;

public sealed record CreateOutputDto(
      string Name,
      int ModuleId,
      short OutputNo,
      int LocationId,
      string Metadata
      );