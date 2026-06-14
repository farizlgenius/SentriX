using System;
using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record UserDto(
      int Id,
      string UserId,
      string Title,
      string FirstName,
      string MiddleName,
      string LastName,
      string Gender,
      DateTime DateOfBirth,
      string Email,
      string Phone,
      int CompanyId,
      int DepartmentId,
      int PositionId,
      string Address,
      List<string> Additionals,
      string Image,
      List<CredentialDto> Credentials,
      List<int> Groups,
      int LocationId,
      bool IsActive
) : BaseDto(
      0,
      LocationId,
      string.Empty,
      IsActive
);