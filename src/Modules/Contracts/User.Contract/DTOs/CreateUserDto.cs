using System;
using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CreateUserDto(
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
      short Flag,
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