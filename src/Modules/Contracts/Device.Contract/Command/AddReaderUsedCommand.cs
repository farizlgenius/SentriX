using SharedKernel.Messaging;

namespace Device.Contract.Command;

public sealed record AddReaderUsedCommand(short ReaderNumber,Guid ModuleGuid,int LocationId) : ICommand;

