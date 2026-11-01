using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.DeviceModule;

public sealed record DeviceModuleNameByExternalIdAndMacAndVendorQuery(short externalId,string mac,Vendor vendor) : IQuery<string>;