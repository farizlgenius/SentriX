using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Output;

public sealed record OutputNameByExternalIdAndMacAndVendorQuery(short externalId,string mac,Vendor vendor) : IQuery<string>;

