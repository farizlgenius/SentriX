using System;
using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CreateUserDto(
      Guid Guid,
      string Identification,
      string Title,
      string FirstName,
      string MiddleName,
      string LastName,
      string Gender,
      DateTime DateOfBirth,
      string Email,
      string Phone,
      Guid CompanyGuid,
      Guid DepartmentGuid,
      Guid PositionGuid,
      string Address,
      DateTime ActiveTime,
      DateTime ExpireTime,
      List<string> Additionals,
      CardDto Card=default!,
      LicensePlateDto LicensePlate=default!,
      QrCodeDto QrCode=default!,
      FaceDto Face=default!,
      PinDto Pin=default!,
      List<Guid> Groups=default!,
      int LocationId=0,
      bool IsActive=true,
      bool IsDefault=false
) : BaseDtoEntity(
      Guid,
      LocationId,
      string.Empty,
      IsActive,
      IsDefault
);