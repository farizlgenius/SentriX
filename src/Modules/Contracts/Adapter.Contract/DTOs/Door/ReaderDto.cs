using SharedKernel.Enums;

namespace Adapter.Contract.DTOs.Door;

public sealed record ReaderDto(
      short SioNo,
      short ReaderNo,
      ReaderMode Mode,
      ReaderDirection ReaderDirection,
      object Metadata
);