using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.MonitorGroup;

public sealed record ComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorQuery(short externalId,short deviceExternalId,Vendor vendor) : IQuery<IEnumerable<(Guid Guid,short ExternalId,MonitorType Type)>>;