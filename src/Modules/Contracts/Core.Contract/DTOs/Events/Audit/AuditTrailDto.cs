using SharedKernel.Enums;

namespace Core.Contract.DTOs.Events.Audit;

public sealed record AuditTrailDto(
      DateTime Timestamp,
      EntityType Entity,
      AuditAction Action,
      string Username,
      string Ip,
      Guid? ObjectGuid,
      string? ObjectName,
      string? Detail
);