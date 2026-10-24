using Core.Contract.DTOs.Device;
using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Device;

public sealed record DeviceByExternalIdAndVendorQuery(short externalId,Vendor Vendor) : IQuery<DeviceDto>;