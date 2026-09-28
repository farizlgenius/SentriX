using SharedKernel.Messaging;

namespace Core.Contract.Commands.ComponentMapping;

public sealed record UpdateExternalIdByMacCommand(string mac,short externalId,CancellationToken ct = default) : ICommand;