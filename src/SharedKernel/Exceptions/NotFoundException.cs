using System;
namespace SharedKernel.Exceptions;

public class NotFoundException : Exception
{
  public NotFoundException() { }
  public NotFoundException(string Message) : base(Message) { }
  public NotFoundException(string Message, Exception innerException) : base(Message, innerException) { }
}
