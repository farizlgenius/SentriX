using System;
using SharedKernel.Domain;

namespace Output.Contract.DTOs;

public sealed record OutputCommandDto(
      int Id = -1,
      short Command = -1,
      string Type = ""
     );
