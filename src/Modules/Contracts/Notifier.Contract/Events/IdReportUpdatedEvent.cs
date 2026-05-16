

using Device.Contract.DTOs;
using SharedKernel.Messaging;

namespace Device.Contract.Events;

public sealed record IdReportUpdatedEvent(List<IdReportDto> Reports) : IEvent;