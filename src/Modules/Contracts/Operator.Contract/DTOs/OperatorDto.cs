using System;

namespace Operator.Contract.DTOs;

public sealed record OperatorDto(
  int Id=0,
  string Username="",
  string Title="",
  string FirstName="",
  string MiddleName="",
  string LastName="",
  string Gender="",
  string Email="",
  string Mobile="",
  int RoleId=0,
  List<int> LocationId=default!,
  bool IsActive=false,
  bool IsDefault=false
);

