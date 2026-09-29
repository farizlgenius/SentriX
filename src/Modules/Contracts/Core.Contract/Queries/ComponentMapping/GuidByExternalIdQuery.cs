using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record GuidByExternalIdAndEntityAndVendorQuery(
      short externalId,
      EntityType entity,
      Vendor vendor) : IQuery<Guid>;