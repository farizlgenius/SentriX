using SharedKernel.Enums;

namespace Core.Contract.DTOs.Events.Audit;

public sealed record AuditTrailDto(
      string Entity,
      AuditAction Action,
      string Username,
      string Ip,
      Guid? ObjectGuid,
      string? ObjectName,
      string? Detail,
      Guid? LocationGuid
);