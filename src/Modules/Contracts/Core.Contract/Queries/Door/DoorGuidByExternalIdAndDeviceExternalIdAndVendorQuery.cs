using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Door;

public sealed record DoorGuidByExternalIdAndDeviceExternalIdAndVendorQuery(short externalId,short deviceExternalId,Vendor vendor) : IQuery<Guid>;