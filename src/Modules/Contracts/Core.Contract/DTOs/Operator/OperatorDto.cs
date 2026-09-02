using SharedKernel.Enums;

namespace Core.Contract.DTOs.Operator;

public sealed record OperatorDto(
      Guid Guid,
  string Username = "",
  Title Title = Title.Mr,
  string FirstName = "",
  string MiddleName = "",
  string LastName = "",
  Gender Gender = Gender.Male,
  string Email = "",
  string Phone = "",
  Guid RoleGuid = default!,
  string Role = "",
  DateTime JoinedDate = default!,
  DateTime ExpiredDate = default!,
  List<Guid> LocationGuids = default!,
  bool IsActive = false,
  bool IsDefault = false
);