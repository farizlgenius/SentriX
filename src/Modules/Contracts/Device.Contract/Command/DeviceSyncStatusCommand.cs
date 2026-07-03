using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record DeviceSyncStatusCommand(string Mac,string Status) : ICommand;