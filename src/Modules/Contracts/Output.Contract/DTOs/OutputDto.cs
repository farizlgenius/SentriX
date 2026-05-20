using System;

namespace Output.Contract.DTOs;

public sealed record OutputDto(int Id,string Name,
      int ModuleId,
      short OutputNo,
      string Model,
      int LocationId,
      int DefaultPulse);
