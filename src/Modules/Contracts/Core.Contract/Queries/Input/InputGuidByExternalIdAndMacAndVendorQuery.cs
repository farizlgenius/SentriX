using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Input;

public sealed record InputGuidByExternalIdAndMacAndVendorQuery(short externalId,string mac,Vendor vendor) : IQuery<Guid>;