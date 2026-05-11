using System;

namespace Operator.Contract.DTOs;

public sealed record OperatorDto(
  int Id,
  string Username,
  string Title,
  string FirstName,
  string MiddleName,
  string LastName,
  string Gender,
  string Email,
  string Mobile,
  int RoleId
);

