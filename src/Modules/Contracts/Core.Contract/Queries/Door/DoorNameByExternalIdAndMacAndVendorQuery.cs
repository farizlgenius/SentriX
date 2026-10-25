using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Door;

public sealed record DoorNameByExternalIdAndMacAndVendorQuery(short externalId,string mac,Vendor Vendor) : IQuery<string>;