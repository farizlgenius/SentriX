namespace SharedKernel.Model;

public sealed record BaseErrorResponse(
      string Exception,
      string? InnerException="",
      string? StackTrace=""
);