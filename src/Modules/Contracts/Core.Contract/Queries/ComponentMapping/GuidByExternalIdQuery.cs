using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record GuidByExternalIdAndEntityAndVendorQuery(
      short externalId,
      string entity,
      Vendor vendor) : IQuery<Guid>;