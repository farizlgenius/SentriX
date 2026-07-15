using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Request;

public sealed record LoginRequest(
      [property: JsonPropertyName("login")]
      string Login,
      [property: JsonPropertyName("password")]
      string Password
);