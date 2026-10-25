using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.DeviceModule;

public sealed record DeviceModuleGuidByExternalIdAndDeviceExternalIdAndVendorQuery(short externalId,short deviceExternalId,Vendor vendor) : IQuery<Guid>;