using SharedKernel.Enums;

namespace Core.Contract.DTOs.Operator;

public sealed record OperatorDto(
  Guid Guid,
  string Username,
  Title Title,
  string FirstName,
  string MiddleName,
  string LastName,
  Gender Gender,
  string Email,
  string Phone,
  DateTime JoinedDate,
  DateTime ExpiredDate,
  BaseDto Role,
  IEnumerable<BaseDto> Locations= default!,
  bool IsActive = true,
  bool IsDefault = false
);