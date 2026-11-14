namespace Core.Contract.DTOs;

public sealed record BaseDto(
      Guid? Guid=null,
      string? Name=null
);