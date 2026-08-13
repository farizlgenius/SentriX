namespace Core.Application.Models.Requests;

public sealed record ExchageReq(
  Guid SessionId,
  string AppDhPub,
  string AppSignPub,
  string Signature
);