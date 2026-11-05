using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Area;

public sealed record AreaGuidByExternalIdAndMacAndVendorQuery(short ExternalId,string Mac,Vendor Vendor) : IQuery<Guid>;