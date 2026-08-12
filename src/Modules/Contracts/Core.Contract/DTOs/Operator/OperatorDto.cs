namespace Core.Contract.DTOs.Operator;

public sealed record OperatorDto(
  Guid Guid,
  string Username,
  string Email,
  string Phone,
  DateTime JoinedDate,
  DateTime ExpiredDate,
  Guid RoleGuid,
  List<Guid> LocationGuids,
  bool IsActive,
  bool IsDefault
);