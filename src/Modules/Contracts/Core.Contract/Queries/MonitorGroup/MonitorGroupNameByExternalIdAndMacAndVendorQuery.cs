using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.MonitorGroup;

public sealed record MonitorGroupNameByExternalIdAndMacAndVendorQuery(short ExternalId,string Mac,Vendor Vendor) : IQuery<string>;