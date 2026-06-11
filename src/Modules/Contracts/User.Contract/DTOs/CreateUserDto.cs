using System;
using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CreateUserDto(
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
      short Flag,
      List<string> additionals,
      string Image,
      List<CredentialDto> credentials,
      List<int> user_groups,
      int LocationId,
      bool IsActive
) : BaseDto(
      0,
      LocationId,
      string.Empty,
      IsActive
);