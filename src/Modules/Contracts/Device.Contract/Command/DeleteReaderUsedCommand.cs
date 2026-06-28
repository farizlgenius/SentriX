using SharedKernel.Messaging;

namespace Device.Contract.Command;
public sealed record DeleteReaderUsedCommand(short ReaderNumber,int ModuleId) : ICommand;