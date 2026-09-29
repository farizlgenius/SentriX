using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.ComponentMapping;

public sealed record MacByExternalIdAndEntityAndVendorQuery(short externalId,EntityType entity,Vendor vendor,CancellationToken ct = default) : IQuery<string>;