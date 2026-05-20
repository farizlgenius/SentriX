using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record ModelByModuleIdQuery(int moduleId) : IQuery<string>;