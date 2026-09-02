using SharedKernel.Enums;

namespace Core.Contract.DTOs.Operator;

public record UpdateOperatorDto(
  Guid Guid,
  string Username,
  Title Title,
  string Firstname,
  string Middlename,
  string Lastname,
  Gender Gender,
  string Email,
  string Phone,
  DateTime JoinedDate,
  DateTime ExpiredDate,
  Guid RoleGuid,
  List<Guid> LocationGuids
  );