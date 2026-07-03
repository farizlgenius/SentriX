namespace Adapter.Amico.Model.Request;

public sealed record LoginRequest(
      string login,
      string passowrd
);