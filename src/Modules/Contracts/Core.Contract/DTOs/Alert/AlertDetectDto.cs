using SharedKernel.Enums;

namespace Core.Contract.DTOs.Alert;

public sealed class AlertDetectDto(
      int externalId,
      EntityType type,
      CoreEvent @event,
      Vendor vendor
);