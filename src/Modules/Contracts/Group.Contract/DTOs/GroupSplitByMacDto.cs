using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record GroupSplitByMacDto(
      string Mac,
      string Type,
      short DeviceComponentId,
      List<short> GroupComponentId
      );

