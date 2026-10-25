using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Device;

public sealed record DeviceGuidByExternalIdAndVendorQuery(short externalId,Vendor vendor) : IQuery<Guid>;