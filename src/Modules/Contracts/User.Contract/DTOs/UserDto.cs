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
    int Id = 0,
    string UserId = "",
    string Title = "",
    string FirstName = "",
    string MiddleName = "",
    string LastName = "",
    string Gender = "",
    DateTime? DateOfBirth = null!,
    string Email = "",
    string Phone = "",
    int CompanyId = 0,
    string Company = "",
    int DepartmentId = 0,
    string Department = "",
    int PositionId = 0,
    string Position = "",
    string Address = "",
    int Flag = 1,
    List<string> Additionals = default!,
    string Image = "",
    List<CredentialDto> Credentials = default!,
    List<int> Groups = default!,
    int VacationId = 0,
    int LocationId = 0,
    bool IsActive = false
) : BaseDto(
      0,
      LocationId,
      string.Empty,
      IsActive
);