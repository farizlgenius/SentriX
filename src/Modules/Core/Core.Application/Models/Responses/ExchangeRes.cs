namespace Core.Application.Models.Responses;

public sealed record ExchangeRes(
  string SessionId,
  string DhPub,
  string SignPub,
  string Signature
);