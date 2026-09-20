using SharedKernel.Enums;

namespace SharedKernel.Model;

public sealed record CommandResponse(
      string Mac,
      short ScpId,
      string Command,
      int Tag,
      DateTime SendAt,
      DateTime? ReceivedAt,
      string Body,
      CommandStatus Status,
      string Reason,
      Vendor Vendor,
      bool IsSend
      );