namespace Core.Contract.DTOs.Operator;

public sealed record UpdateOperatorDto(
  Guid Guid,
  string Username,
  string Email,
  string Phone,
  DateTime JoinedDate,
  DateTime ExpiredDate,
  Guid RoleGuid,
  List<Guid> LocationGuids
);