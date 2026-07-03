using SharedKernel.Domain;

namespace Events.Contract.DTOs;

public sealed record CommandEventDto(
      int Id,
      string Name,
      string Mac,
      string Command,
      int Tag,
      DateTime Send,
      DateTime Received,
      string Body,
      string Status,
      string Reason,
      string Response,
            int LocationId,
      string Type,
      bool IsActive
) : BaseDto(0,LocationId,Type,IsActive);