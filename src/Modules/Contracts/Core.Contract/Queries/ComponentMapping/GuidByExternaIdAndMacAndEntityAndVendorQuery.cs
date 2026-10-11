using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record GuidByExternalIdAndMacAndEntityAndVendorQuery(
      short externalId,
      string mac,
      EntityType entity,
      Vendor vendor) : IQuery<Guid>;