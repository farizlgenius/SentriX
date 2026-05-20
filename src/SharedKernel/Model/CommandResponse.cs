namespace SharedKernel.Model;

public sealed record CommandResponse(
      string Mac,
      short ScpId,
      string Command,
      int Tag,
      DateTime SendAt,
      DateTime ReceivedAt,
      string Body,
      string Status,
      string Reason,
      bool IsSend
      );