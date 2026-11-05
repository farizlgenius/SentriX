using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Door;

public sealed record DoorGuidByExternalIdAndMacAndVendorQuery(short externalId,string mac,Vendor vendor) : IQuery<Guid>;