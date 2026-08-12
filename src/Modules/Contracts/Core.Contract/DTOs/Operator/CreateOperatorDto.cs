namespace Core.Contract.DTOs.Operator;

public sealed record CreateOperatorDto(
  string Username,
  string Password,
  string Email,
  string Phone,
  DateTime JoinedDate,
  DateTime ExpiredDate,
  Guid RoleGuid,
  List<Guid> LocationGuids
);