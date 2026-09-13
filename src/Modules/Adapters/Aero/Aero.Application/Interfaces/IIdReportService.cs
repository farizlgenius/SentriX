using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IIdReportService
{
  Task HandleInCommingDeviceAsync(ReplyMessage.SCPReplyIDReport dto, CancellationToken ct = default);
}