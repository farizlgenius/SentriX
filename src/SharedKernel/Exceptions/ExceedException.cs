using System;

namespace SharedKernel.Exceptions;

public sealed class ExceedException : Exception
{
  public ExceedException() { }
  public ExceedException(string Entity, string Data) : base($"Component exceed limit on {Entity}:{Data}") { }
  public ExceedException(string Message) : base(Message) { }
  public ExceedException(string Message, Exception innerException) : base(Message, innerException) { }
}
