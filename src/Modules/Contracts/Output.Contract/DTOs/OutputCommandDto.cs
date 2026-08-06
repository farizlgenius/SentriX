using System;
using SharedKernel.Domain;

namespace Output.Contract.DTOs;

public sealed record OutputCommandDto(
      Guid Guid = default,
      short Command = -1,
      string Type = ""
     );
