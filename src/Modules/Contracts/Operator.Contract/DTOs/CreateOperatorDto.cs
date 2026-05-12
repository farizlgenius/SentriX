using System;

namespace Operator.Contract.DTOs;

public record CreateOperatorDto(
  string Username,
  string Password,
  string title,
  string Firstname,
  string Middlename,
  string Lastname,
  string Gender,
  string Email,
  string Mobile,
  int RoleId,
  List<int> LocationId
  );
