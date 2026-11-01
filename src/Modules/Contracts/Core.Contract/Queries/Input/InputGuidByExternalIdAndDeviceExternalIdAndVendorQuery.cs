using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Input;

public sealed record InputGuidByExternalIdAndDeviceExternalIdAndVendorQuery(short externalId,short deivceExternalId,Vendor vendor) : IQuery<Guid>;