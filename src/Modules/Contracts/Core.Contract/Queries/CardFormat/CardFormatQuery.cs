using Core.Contract.DTOs.CardFormat;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.CardFormat;

public sealed record CardFormatQuery() : IQuery<IEnumerable<CardFormatDto>>;