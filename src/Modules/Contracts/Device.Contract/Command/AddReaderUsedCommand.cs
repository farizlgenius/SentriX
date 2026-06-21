using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record AddReaderUsedCommand(short ReaderNumber,int ModuleId,int LocationId) : ICommand<bool>;