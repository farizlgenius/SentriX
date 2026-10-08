using SharedKernel.Enums;

namespace Core.Contract.DTOs.User;

public sealed record UpdateUserDto(
  Guid Guid,
  string UserCode,
  string Username,
  string Identification,
  Title Title,
  string Firstname,
  string Middlename,
  string Lastname,
  Gender Gender,
  DateTime DateOfBirth,
  string Email,
  string Phone,
  string Address,
  DateTime JoinedDate,
  DateTime ExpiredDate,
  List<string> Additionals,
  List<Guid> Groups,
  List<CardDto> Cards,
 LicensePlateDto LicensePlate,
  PinDto Pin,
  QrCodeDto QrCode,
  FaceDto Face,
  List<Guid> Locations,
  string Metadata,
   Guid? RoleGuid=null,
  Guid? CompanyGuid=null,
  Guid? DepartmentGuid=null,
  Guid? PositionGuid=null
);