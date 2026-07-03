using Setting.Contract.DTOs;
using SharedKernel.Messaging;

namespace Setting.Contract.Queries;

public sealed record CardFormatByLocationIdQuery(
      int LocationId
) : IQuery<IEnumerable<CardFormatDto>>;