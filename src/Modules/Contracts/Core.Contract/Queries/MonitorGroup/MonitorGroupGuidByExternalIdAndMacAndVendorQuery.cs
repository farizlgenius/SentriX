using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.MonitorGroup;

public sealed record MonitorGroupGuidByExternalIdAndMacAndVendorQuery(
      short externalId,
      string mac,
      Vendor vendor
) : IQuery<Guid>;