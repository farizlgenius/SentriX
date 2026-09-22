using SharedKernel.Enums;

namespace Core.Contract.DTOs.Events.AdapterEvent;

public sealed record AdapterEventDto(
Guid Guid,
string Name,
string Mac,
int ComponentId,
string Command,
int Tag,
DateTime SendAt,
DateTime? ReceivedAt,
string Body,
CommandStatus Status,
string Reason,
string Response,
Vendor Vendor,
Guid LocationGuid,
bool IsActive,
bool IsDefault
);