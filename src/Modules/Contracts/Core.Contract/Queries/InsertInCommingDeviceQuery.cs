using Core.Contract.DTOs.Device;
using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record InsertInCommingDeviceQuery(CreateDeviceDto device) : IQuery<bool>;