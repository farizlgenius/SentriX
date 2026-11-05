using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Output;

public sealed record OutputGuidByExternalIdAndMacAndVendorQuery(short externalId,string mac,Vendor vendor) : IQuery<Guid>;