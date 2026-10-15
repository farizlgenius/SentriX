namespace Core.Contract.DTOs.Alert;


public sealed record UpdateAlertLevelDto(
      Guid Guid,
      string Name,
      string ColorCode
);