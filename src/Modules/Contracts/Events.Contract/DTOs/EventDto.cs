using System;

namespace Events.Contract.DTOs;

public sealed record EventDto(
      DateTime Timestamp,
      string Actor,
      string Module,
      string Type,
      string Image,
      string Mac,
      string Name,
      string Remarks,
      int LocationId
      );
