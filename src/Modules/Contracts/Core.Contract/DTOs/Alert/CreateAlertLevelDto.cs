namespace Core.Contract.DTOs.Alert;


public sealed record CreateAlertLevelDto(
      string Name,
      string ColorCode
);