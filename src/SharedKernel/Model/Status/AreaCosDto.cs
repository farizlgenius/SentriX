using SharedKernel.Enums;

namespace SharedKernel.Model.Status;

public sealed record AreaCosDto(
      Guid Guid,
      bool Enabled,
      bool MultiOccupancy,
      bool NotConfigure,
      int Occupancy
);