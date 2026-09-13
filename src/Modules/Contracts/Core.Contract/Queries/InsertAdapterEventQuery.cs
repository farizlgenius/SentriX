using SharedKernel.Messaging;
using SharedKernel.Model;

namespace Core.Contract.Queries;

public sealed record InsertAdapterEventQuery(
  CommandResponse res
) : IQuery<bool>;