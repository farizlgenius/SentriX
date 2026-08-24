using SharedKernel.Enums;

namespace Core.Contract.DTOs.User;

public sealed record CreateUserDto(
  string Username,
  string Password,
  string Identification,
  Title Title,
  string Firstname,
  string Middlename,
  string Lastname,
  Gender Gender,
  DateTime DateOfBirth,
  string Email,
  string Phone,
  bool IsOperator,
  bool IsUser,
  Guid RoleGuid,
  Guid CompanyGuid,
  Guid DepartmentGuid,
  Guid PositionGuid,
  string Address,
  DateTime JoinedDate,
  DateTime ExpiredDate,
  List<string> Additionals,
  List<Guid> Groups,
  List<CardDto> Cards,
  List<LicensePlateDto> LicensePlates,
  PinDto Pin,
  List<QrCodeDto> QrCodes,
  FaceDto Face,
  List<Guid> Locations
);