using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Output;

public sealed record OutputGuidByExternalIdAndDeviceExternalIdAndVendorQuery(short externalId,short deivceExternalId,Vendor vendor) : IQuery<Guid>;