using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed record CoreEventDto(
      Guid Guid,
      CoreEntityDto Entity,
      CoreEvent Event
);