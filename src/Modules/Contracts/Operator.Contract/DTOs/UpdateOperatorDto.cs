using System;

namespace Operator.Contract.DTOs;

public record UpdateOperatorDto(
  int Id,
  string Username,
  string Title,
  string Firstname,
  string Middlename,
  string Lastname,
  string Gender,
  string Email,
  string Mobile,
  int RoleId,
  List<int> LocationId
  );