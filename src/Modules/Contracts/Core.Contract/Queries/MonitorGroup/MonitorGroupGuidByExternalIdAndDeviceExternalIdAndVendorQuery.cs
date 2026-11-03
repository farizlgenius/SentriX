using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.MonitorGroup;

public sealed record MonitorGroupGuidByExternalIdAndDeviceExternalIdAndVendorQuery(
      short externalId,
      short deviceExternalId,
      Vendor vendor
) : IQuery<Guid>;