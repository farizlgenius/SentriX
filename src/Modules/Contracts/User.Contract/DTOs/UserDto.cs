using System;
using SharedKernel.Domain;

namespace User.Contract.DTOs;

// public sealed record UserDto(
//       int Id,
//       string UserId,
//       string Title,
//       string FirstName,
//       string MiddleName,
//       string LastName,
//       string Gender,
//       DateTime DateOfBirth,
//       string Email,
//       string Phone,
//       int CompanyId,
//       string Company,
//       int DepartmentId,
//       string Department,
//       int PositionId,
//       string Position,
//       string Address,
//       List<string> Additionals,
//       string Image,
//       List<CredentialDto> Credentials,
//       List<int> Groups,
//       int LocationId,
//       bool IsActive
// ) : BaseDto(
//       0,
//       LocationId,
//       string.Empty,
//       IsActive
// );

public sealed record UserDto(
    Guid Guid = default!,
    string Identification = "",
    string Title = "",
    string FirstName = "",
    string MiddleName = "",
    string LastName = "",
    string Gender = "",
    DateTime? DateOfBirth = null!,
    string Email = "",
    string Phone = "",
    Guid CompanyGuid = default,
    string Company = "",
    Guid DepartmentGuid = default,
    string Department = "",
    Guid PositionGuid = default,
    string Position = "",
    string Address = "",
    DateTime ActiveTime = default,
    DateTime ExpireTime = default,
    List<string> Additionals = default!,
    CardDto Card=default!,
      LicensePlateDto LicensePlate=default!,
      QrCodeDto QrCode=default!,
      FaceDto Face=default!,
      PinDto Pin=default!,
    List<Guid> Groups = default!,
    int LocationId = 0,
    bool IsActive = true,
    bool IsDefault=false
) : BaseDtoEntity(
    Guid,
      LocationId,
      string.Empty,
      IsActive,
      IsDefault
);