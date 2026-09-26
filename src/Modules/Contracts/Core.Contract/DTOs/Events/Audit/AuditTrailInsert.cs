using SharedKernel.Enums;

namespace Core.Contract.DTOs.Events.Audit;

public sealed record AuditTrailInsert(
      string Entity,
      AuditAction Action,
      string Username,
      string Ip,
      Guid? ObjectGuid,
      string? ObjectName,
      string? Detail,
      int LocationId
);