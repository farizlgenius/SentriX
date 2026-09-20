using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record InternalIdByExternalIdAndEntityAndVendorQuery(
      short externalId,
      string entity,
      Vendor vendor
) : IQuery<int>;