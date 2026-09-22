using SharedKernel.Enums;

namespace Core.Contract.DTOs.Events.ExceptionEvent;

public sealed record ExceptionEventDto(
      DateTime Timestamp,
      string Entity,
      string Exception,
      string InnerException,
      string StackTrace
);