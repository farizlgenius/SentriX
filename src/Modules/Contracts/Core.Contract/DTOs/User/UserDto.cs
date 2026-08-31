using SharedKernel.Enums;

namespace Core.Contract.DTOs.User;

public sealed record UserDto(
  Guid Guid,
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
  bool IsOperator,
  bool IsUser,
  string Role,
  string Company,
  string Department,
  string Position,
  string Address,
  DateTime JoinedDate,
  DateTime ExpiredDate,
  List<string> Additionals,
  List<string> Groups,
  List<CardDto> Cards,
  LicensePlateDto LicensePlate,
  PinDto Pin,
  QrCodeDto QrCode,
  FaceDto Face,
  List<string> Locations
);