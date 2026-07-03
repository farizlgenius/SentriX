using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record DeviceSyncTimeCommand(string Mac) : ICommand;