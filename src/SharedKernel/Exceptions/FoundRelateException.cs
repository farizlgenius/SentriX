using System;
namespace SharedKernel.Exceptions;

public class FoundRelateException : Exception
{
  public Dictionary<string,IEnumerable<Guid>> Payload { get;} = default!;

  public FoundRelateException() { }
  public FoundRelateException(string Message) : base(Message) { }
  public FoundRelateException(string Entity, string Data, string Relate) : base($"Found {Relate} relate with {Entity}:{Data}") { }
  public FoundRelateException(string Message, Exception innerException) : base(Message, innerException) { }
  public FoundRelateException(Dictionary<string,IEnumerable<Guid>> payload)
  {
    Payload = payload;
  }
}
