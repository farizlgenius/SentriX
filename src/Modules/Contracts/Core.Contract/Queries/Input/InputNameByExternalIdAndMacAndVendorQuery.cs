using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Input;

public sealed record InputNameByExternalIdAndMacAndVendorQuery(short externalId,string mac,Vendor vendor) : IQuery<string>;