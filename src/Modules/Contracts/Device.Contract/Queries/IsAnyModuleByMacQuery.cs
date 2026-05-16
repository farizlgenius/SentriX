using System;
using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record IsAnyModuleBySerialNumberQuery(string SerialNumber) : IQuery<bool>;
