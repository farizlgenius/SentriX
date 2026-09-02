namespace Core.Contract.DTOs.Operator;

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