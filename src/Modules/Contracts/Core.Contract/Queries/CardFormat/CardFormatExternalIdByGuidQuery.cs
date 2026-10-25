using SharedKernel.Messaging;

namespace Core.Contract.Queries.CardFormat;

public sealed record CardFormatExternalIdByGuidQuery(Guid guid) : IQuery<short>;