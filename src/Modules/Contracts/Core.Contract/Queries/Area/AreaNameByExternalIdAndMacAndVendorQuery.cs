using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Area;

public sealed record AreaNameByExternalIdAndMacAndVendorQuery(short ExternalId,string Mac,Vendor Vendor) : IQuery<string>;