using System;
using SharedKernel.Messaging;

namespace Device.Contract.Queries;


public sealed record DeviceMacByComponentIdQuery(int ComponentId) : IQuery<string>;